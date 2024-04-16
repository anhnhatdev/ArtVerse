'use client';
import { create } from 'zustand';
import { User, Role } from '@/types/api';
import { apiClient } from './api-client';

interface AuthState {
  user: User | null;
  token: string | null;
  isLoading: boolean;
  loginWithRole: (role: Role) => Promise<void>;
  logout: () => void;
  initialize: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: null,
  isLoading: false,

  initialize: () => {
    if (typeof window === 'undefined') return;
    const token = localStorage.getItem('artverse_token');
    const userStr = localStorage.getItem('artverse_user');
    if (token && userStr) {
      try {
        set({ token, user: JSON.parse(userStr) });
      } catch (e) {
        localStorage.removeItem('artverse_token');
        localStorage.removeItem('artverse_user');
      }
    }
  },

  loginWithRole: async (role: Role) => {
    set({ isLoading: true });
    try {
      const res = await apiClient.post('/auth/quick-login', { role });
      const { token, user } = res.data;
      localStorage.setItem('artverse_token', token);
      localStorage.setItem('artverse_user', JSON.stringify(user));
      set({ token, user, isLoading: false });
    } catch (err) {
      console.error('Quick login error:', err);
      // Fallback mock session for smooth frontend testing
      const fallbackUser: User = {
        id: 'usr-1',
        userName: role.toLowerCase(),
        email: `${role.toLowerCase()}@artverse.edu.vn`,
        fullName: `Demo ${role} Account`,
        role: role,
        avatarUrl: `https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150&auto=format&fit=crop&q=80`
      };
      set({ token: 'mock-jwt-token', user: fallbackUser, isLoading: false });
      localStorage.setItem('artverse_user', JSON.stringify(fallbackUser));
    }
  },

  logout: () => {
    localStorage.removeItem('artverse_token');
    localStorage.removeItem('artverse_user');
    set({ user: null, token: null });
  },
}));