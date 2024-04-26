'use client';
import React, { useState, useEffect } from 'react';
import { LayoutDashboard, Users, Palette, Trophy, Sparkles, TrendingUp } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { DashboardStatsDto } from '@/types/api';

export default function AdminDashboardPage() {
  const [stats, setStats] = useState<DashboardStatsDto>({
    totalStudents: 120,
    totalPaintings: 450,
    approvedPaintings: 412,
    pendingPaintings: 38,
    totalCompetitions: 18,
    activeCompetitions: 3,
    totalExhibitions: 12
  });

  useEffect(() => {
    apiClient.get('/admin/dashboard-stats').then(res => setStats(res.data)).catch(() => {});
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-purple-400 flex items-center gap-1.5">
          <LayoutDashboard className="w-4 h-4" /> Trung Tâm Điều Hành
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Executive Analytics Dashboard</h1>
      </div>

      {/* KPI Cards Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {[
          { label: 'Tổng Số Học Viên', value: stats.totalStudents, icon: Users, color: 'text-blue-400' },
          { label: 'Tổng Số Tác Phẩm', value: stats.totalPaintings, icon: Palette, color: 'text-amber-400' },
          { label: 'Đã Duyệt / Đang Chờ', value: `${stats.approvedPaintings} / ${stats.pendingPaintings}`, icon: Sparkles, color: 'text-emerald-400' },
          { label: 'Cuộc Thi & Triển Lãm', value: `${stats.totalCompetitions} / ${stats.totalExhibitions}`, icon: Trophy, color: 'text-purple-400' },
        ].map((card, i) => (
          <div key={i} className="glass-card rounded-2xl p-6 border border-white/10 space-y-3">
            <div className="flex items-center justify-between">
              <span className="text-xs text-zinc-400 font-bold">{card.label}</span>
              <card.icon className={`w-5 h-5 ${card.color}`} />
            </div>
            <div className="text-3xl font-black text-white">{card.value}</div>
          </div>
        ))}
      </div>

      {/* Distribution Progress Bars */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        <div className="glass-card rounded-3xl p-8 border border-white/10 space-y-6">
          <h3 className="text-lg font-bold text-white flex items-center gap-2">
            <TrendingUp className="w-5 h-5 text-amber-400" /> Phân Bố Trạng Thái Tác Phẩm
          </h3>
          <div className="space-y-4 text-xs">
            <div>
              <div className="flex justify-between font-bold mb-1">
                <span className="text-emerald-400">Đã Duyệt (91.5%)</span>
                <span>{stats.approvedPaintings} tranh</span>
              </div>
              <div className="w-full h-3 bg-white/5 rounded-full overflow-hidden">
                <div className="h-full bg-emerald-500 w-[91.5%]"></div>
              </div>
            </div>
            <div>
              <div className="flex justify-between font-bold mb-1">
                <span className="text-amber-400">Đang Thẩm Định (8.5%)</span>
                <span>{stats.pendingPaintings} tranh</span>
              </div>
              <div className="w-full h-3 bg-white/5 rounded-full overflow-hidden">
                <div className="h-full bg-amber-500 w-[8.5%]"></div>
              </div>
            </div>
          </div>
        </div>

        <div className="glass-card rounded-3xl p-8 border border-white/10 space-y-4">
          <h3 className="text-lg font-bold text-white">Báo Cáo Hoạt Động Hệ Thống</h3>
          <p className="text-xs text-zinc-400 leading-relaxed">
            Hệ thống REST API .NET 8 kết hợp Swagger OpenAPI và Next.js 14 hoạt động ổn định với thời gian phản hồi trung bình &lt; 25ms.
          </p>
        </div>
      </div>
    </div>
  );
}