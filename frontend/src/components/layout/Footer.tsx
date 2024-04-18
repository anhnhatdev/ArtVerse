import React from 'react';
import Link from 'next/link';
import { Palette, Heart, ShieldCheck, Sparkles } from 'lucide-react';

export default function Footer() {
  return (
    <footer className="border-t border-white/10 bg-[#06070a] text-zinc-400 pt-16 pb-12 mt-24">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-10 pb-12 border-b border-white/10">
          
          {/* Brand Col */}
          <div className="space-y-4 md:col-span-1">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-amber-400 to-amber-600 flex items-center justify-center">
                <Palette className="w-5 h-5 text-black font-bold" />
              </div>
              <span className="text-xl font-black text-white">Art<span className="gold-gradient-text">Verse</span></span>
            </div>
            <p className="text-xs leading-relaxed text-zinc-400">
              Nền tảng Quản lý & Triển lãm Tranh Học đường Cao Cấp. Kiến trúc Clean Architecture .NET 8 kết hợp Next.js 14.
            </p>
          </div>

          {/* Links 1 */}
          <div>
            <h4 className="text-sm font-bold text-white uppercase tracking-wider mb-4">Khám Phá</h4>
            <ul className="space-y-2 text-xs">
              <li><Link href="/artworks" className="hover:text-amber-400 transition-colors">Thư viện tranh xuất sắc</Link></li>
              <li><Link href="/competitions" className="hover:text-amber-400 transition-colors">Cuộc thi & Hội đồng giám khảo</Link></li>
              <li><Link href="/exhibitions" className="hover:text-amber-400 transition-colors">Triển lãm không gian số</Link></li>
              <li><Link href="/students" className="hover:text-amber-400 transition-colors">Danh bạ họa sĩ trẻ</Link></li>
            </ul>
          </div>

          {/* Links 2 */}
          <div>
            <h4 className="text-sm font-bold text-white uppercase tracking-wider mb-4">Dành Cho Nhà Trường</h4>
            <ul className="space-y-2 text-xs">
              <li><Link href="/curation/review-queue" className="hover:text-amber-400 transition-colors">Hội Đồng Cố Vấn Duyệt Tranh</Link></li>
              <li><Link href="/admin/dashboard" className="hover:text-amber-400 transition-colors">Trung Tâm Điều Hành KPI</Link></li>
              <li><Link href="/academic/classes" className="hover:text-amber-400 transition-colors">Quản Lý Lớp & Niên Khóa</Link></li>
              <li><Link href="/auth/login" className="hover:text-amber-400 transition-colors">1-Click Role Switcher Demo</Link></li>
            </ul>
          </div>

          {/* Tech Badge */}
          <div className="space-y-3">
            <h4 className="text-sm font-bold text-white uppercase tracking-wider mb-4">Tiêu Chuẩn Công Nghệ</h4>
            <div className="flex items-center gap-2 text-xs text-emerald-400 bg-emerald-500/10 border border-emerald-500/20 px-3 py-2 rounded-lg">
              <ShieldCheck className="w-4 h-4" />
              <span>JWT Bearer + RBAC Policies</span>
            </div>
            <div className="flex items-center gap-2 text-xs text-amber-400 bg-amber-500/10 border border-amber-500/20 px-3 py-2 rounded-lg">
              <Sparkles className="w-4 h-4" />
              <span>Clean Architecture .NET 8 + Next.js</span>
            </div>
          </div>

        </div>

        <div className="pt-8 flex flex-col sm:flex-row items-center justify-between text-xs text-zinc-500 gap-4">
          <p>© 2024 ArtVerse Enterprise Academy. All rights reserved.</p>
          <p className="flex items-center gap-1">
            Thiết kế & phát triển bởi <span className="text-zinc-300 font-semibold">anhnhatdev</span>
          </p>
        </div>
      </div>
    </footer>
  );
}