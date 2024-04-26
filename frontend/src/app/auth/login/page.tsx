'use client';
import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/lib/auth-store';
import { Role } from '@/types/api';
import { Lock, ShieldCheck, UserCheck, Sparkles, LogIn } from 'lucide-react';

export default function LoginPage() {
  const router = useRouter();
  const { loginWithRole, isLoading } = useAuthStore();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const demoRoles: { role: Role; title: string; desc: string; color: string }[] = [
    { role: 'Admin', title: 'Quản Trị Viên (Dean / Admin)', desc: 'Toàn quyền cấu hình hệ thống, KPI dashboard & phân quyền', color: 'border-purple-500/40 bg-purple-500/10 text-purple-300' },
    { role: 'Curator', title: 'Hội Đồng Giám Tuyển (Curator)', desc: 'Thẩm định nghệ thuật, duyệt tranh vào Gallery & tổ chức triển lãm', color: 'border-emerald-500/40 bg-emerald-500/10 text-emerald-300' },
    { role: 'Teacher', title: 'Giảng Viên / Giám Khảo', desc: 'Chấm thi theo thang Rubric & quản lý lớp học', color: 'border-blue-500/40 bg-blue-500/10 text-blue-300' },
    { role: 'Student', title: 'Họa Sĩ Học Viên', desc: 'Nộp tranh chất lượng cao & tham gia các cuộc thi', color: 'border-amber-500/40 bg-amber-500/10 text-amber-300' },
  ];

  const handleQuickLogin = async (role: Role) => {
    await loginWithRole(role);
    if (role === 'Admin') router.push('/admin/dashboard');
    else if (role === 'Curator') router.push('/curation/review-queue');
    else if (role === 'Student') router.push('/studio/upload');
    else router.push('/artworks');
  };

  return (
    <div className="max-w-4xl mx-auto px-4 py-16 space-y-12">
      <div className="text-center space-y-3">
        <div className="inline-flex items-center gap-2 px-3.5 py-1 rounded-full bg-amber-500/10 text-amber-400 text-xs font-bold border border-amber-500/20">
          <ShieldCheck className="w-4 h-4" /> Hệ Thống Bảo Mật JWT RBAC
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white">Đăng Nhập Trải Nghiệm ArtVerse</h1>
        <p className="text-zinc-400 text-sm max-w-xl mx-auto">
          Chọn 1-Click Role Switcher bên dưới để tự động cấp quyền và đăng nhập trải nghiệm ngay lập tức.
        </p>
      </div>

      {/* 1-Click Role Switcher Cards Grid */}
      <div className="space-y-4">
        <h3 className="text-xs font-bold uppercase tracking-wider text-zinc-400 text-center">
          ⚡ 1-Click Role Switcher Demo
        </h3>
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          {demoRoles.map((item) => (
            <button
              key={item.role}
              onClick={() => handleQuickLogin(item.role)}
              disabled={isLoading}
              className={`p-6 rounded-2xl border text-left transition-all hover:scale-[1.02] flex flex-col justify-between space-y-3 ${item.color}`}
            >
              <div>
                <div className="flex items-center justify-between">
                  <span className="text-xs font-black uppercase tracking-wider">{item.role}</span>
                  <Sparkles className="w-4 h-4" />
                </div>
                <div className="text-base font-bold text-white mt-1">{item.title}</div>
                <p className="text-xs opacity-80 mt-1">{item.desc}</p>
              </div>
              <div className="text-xs font-bold underline flex items-center gap-1">
                Đăng nhập với vai trò này →
              </div>
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}