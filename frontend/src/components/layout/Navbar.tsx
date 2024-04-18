'use client';
import React, { useEffect, useState } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { useAuthStore } from '@/lib/auth-store';
import { 
  Palette, 
  Trophy, 
  Sparkles, 
  Users, 
  UploadCloud, 
  ShieldCheck, 
  LogIn, 
  LogOut, 
  UserCheck,
  GraduationCap,
  LayoutDashboard,
  Menu,
  X
} from 'lucide-react';
import { Role } from '@/types/api';

export default function Navbar() {
  const pathname = usePathname();
  const { user, loginWithRole, logout, initialize } = useAuthStore();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [roleDropdownOpen, setRoleDropdownOpen] = useState(false);

  useEffect(() => {
    initialize();
  }, [initialize]);

  const navLinks = [
    { href: '/artworks', label: 'Thư Viện Tranh', icon: Palette },
    { href: '/competitions', label: 'Cuộc Thi & Xếp Hạng', icon: Trophy },
    { href: '/exhibitions', label: 'Triển Lãm Số', icon: Sparkles },
    { href: '/students', label: 'Họa Sĩ Trẻ', icon: Users },
  ];

  const roles: Role[] = ['Admin', 'Curator', 'Teacher', 'Student', 'Guest'];

  return (
    <nav className="sticky top-0 z-50 glass-card border-b border-white/10 bg-[#090a0f]/90">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-20">
          
          {/* Brand Logo */}
          <Link href="/" className="flex items-center gap-3 group">
            <div className="w-11 h-11 rounded-xl bg-gradient-to-br from-amber-400 via-amber-500 to-amber-700 flex items-center justify-center shadow-lg shadow-amber-500/20 group-hover:scale-105 transition-transform">
              <Palette className="w-6 h-6 text-black font-bold" />
            </div>
            <div>
              <span className="text-2xl font-black tracking-tight text-white flex items-center gap-1.5">
                Art<span className="gold-gradient-text">Verse</span>
              </span>
              <span className="text-[10px] tracking-widest text-zinc-400 uppercase font-semibold block">Fine Arts Academy</span>
            </div>
          </Link>

          {/* Desktop Nav Links */}
          <div className="hidden md:flex items-center gap-1 lg:gap-2">
            {navLinks.map((link) => {
              const Icon = link.icon;
              const isActive = pathname.startsWith(link.href);
              return (
                <Link
                  key={link.href}
                  href={link.href}
                  className={`flex items-center gap-2 px-3.5 py-2 rounded-lg text-sm font-medium transition-all ${
                    isActive 
                      ? 'bg-amber-500/10 text-amber-400 border border-amber-500/20' 
                      : 'text-zinc-300 hover:text-white hover:bg-white/5'
                  }`}
                >
                  <Icon className="w-4 h-4" />
                  {link.label}
                </Link>
              );
            })}

            {/* Quick role shortcuts for specific roles */}
            {user?.role === 'Admin' && (
              <Link
                href="/admin/dashboard"
                className="flex items-center gap-1.5 px-3 py-2 rounded-lg text-sm font-medium text-purple-400 hover:bg-purple-500/10 border border-purple-500/20"
              >
                <LayoutDashboard className="w-4 h-4" />
                Quản Trị
              </Link>
            )}

            {user?.role === 'Curator' && (
              <Link
                href="/curation/review-queue"
                className="flex items-center gap-1.5 px-3 py-2 rounded-lg text-sm font-medium text-emerald-400 hover:bg-emerald-500/10 border border-emerald-500/20"
              >
                <ShieldCheck className="w-4 h-4" />
                Duyệt Tranh
              </Link>
            )}

            {user?.role === 'Student' && (
              <Link
                href="/studio/upload"
                className="flex items-center gap-1.5 px-3 py-2 rounded-lg text-sm font-medium text-amber-400 hover:bg-amber-500/10 border border-amber-500/20"
              >
                <UploadCloud className="w-4 h-4" />
                Nộp Tác Phẩm
              </Link>
            )}
          </div>

          {/* User Auth & 1-Click Role Switcher */}
          <div className="hidden md:flex items-center gap-3">
            
            {/* 1-Click Role Switcher Dropdown */}
            <div className="relative">
              <button
                onClick={() => setRoleDropdownOpen(!roleDropdownOpen)}
                className="flex items-center gap-2 px-3 py-1.5 rounded-lg text-xs font-semibold bg-white/5 hover:bg-white/10 border border-white/10 text-zinc-300 transition-colors"
                title="Chuyển đổi vai trò trải nghiệm nhanh"
              >
                <UserCheck className="w-3.5 h-3.5 text-amber-400" />
                <span>Vai trò: <strong className="text-amber-400">{user?.role || 'Khách'}</strong></span>
              </button>

              {roleDropdownOpen && (
                <div className="absolute right-0 mt-2 w-52 glass-card rounded-xl p-2 shadow-2xl border border-white/15 z-50 bg-[#111420]">
                  <div className="px-3 py-2 text-[11px] font-bold uppercase tracking-wider text-zinc-400 border-b border-white/5">
                    Demo Role Switcher
                  </div>
                  {roles.map((r) => (
                    <button
                      key={r}
                      onClick={() => {
                        loginWithRole(r);
                        setRoleDropdownOpen(false);
                      }}
                      className={`w-full text-left px-3 py-2 rounded-lg text-xs font-medium flex items-center justify-between transition-colors ${
                        user?.role === r ? 'bg-amber-500/20 text-amber-400' : 'text-zinc-300 hover:bg-white/5 hover:text-white'
                      }`}
                    >
                      <span>{r}</span>
                      {user?.role === r && <span className="w-1.5 h-1.5 rounded-full bg-amber-400"></span>}
                    </button>
                  ))}
                </div>
              )}
            </div>

            {user ? (
              <div className="flex items-center gap-3 pl-2 border-l border-white/10">
                <div className="text-right">
                  <div className="text-xs font-bold text-white leading-none">{user.fullName}</div>
                  <div className="text-[10px] text-amber-400 font-medium">{user.role}</div>
                </div>
                <button
                  onClick={logout}
                  className="p-2 rounded-lg text-zinc-400 hover:text-red-400 hover:bg-red-500/10 transition-colors"
                  title="Đăng xuất"
                >
                  <LogOut className="w-4 h-4" />
                </button>
              </div>
            ) : (
              <Link
                href="/auth/login"
                className="flex items-center gap-1.5 px-4 py-2 rounded-lg text-xs font-bold bg-amber-500 text-black hover:bg-amber-400 transition-colors shadow-lg shadow-amber-500/20"
              >
                <LogIn className="w-3.5 h-3.5" />
                Đăng Nhập
              </Link>
            )}
          </div>

          {/* Mobile Menu Toggle Button */}
          <div className="flex md:hidden items-center gap-2">
            <button
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
              className="p-2 rounded-lg text-zinc-400 hover:text-white hover:bg-white/5"
            >
              {mobileMenuOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
            </button>
          </div>

        </div>
      </div>

      {/* Mobile Menu Dropdown */}
      {mobileMenuOpen && (
        <div className="md:hidden border-t border-white/10 bg-[#090a0f] px-4 py-4 space-y-2">
          {navLinks.map((link) => (
            <Link
              key={link.href}
              href={link.href}
              onClick={() => setMobileMenuOpen(false)}
              className="flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium text-zinc-300 hover:bg-white/5"
            >
              <link.icon className="w-5 h-5 text-amber-400" />
              {link.label}
            </Link>
          ))}
          <div className="pt-3 border-t border-white/10 flex items-center justify-between">
            <span className="text-xs text-zinc-400">Đang là: <strong className="text-amber-400">{user?.role || 'Khách'}</strong></span>
            {user ? (
              <button onClick={logout} className="text-xs text-red-400 hover:underline font-bold">Đăng xuất</button>
            ) : (
              <Link href="/auth/login" className="text-xs text-amber-400 font-bold hover:underline">Đăng nhập</Link>
            )}
          </div>
        </div>
      )}
    </nav>
  );
}