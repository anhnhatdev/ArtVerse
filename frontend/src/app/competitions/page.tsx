'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { Trophy, Calendar, Award, ArrowRight } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { CompetitionDto } from '@/types/api';

export default function CompetitionsPage() {
  const [competitions, setCompetitions] = useState<CompetitionDto[]>([]);
  useEffect(() => {
    apiClient.get('/competitions').then(res => setCompetitions(res.data || [])).catch(() => {
      setCompetitions([
        {
          id: 'c-1',
          title: 'Cuộc Thi Sáng Tác Mỹ Thuật Trẻ Toàn Quốc 2024',
          description: 'Tôn vinh tài năng hội họa trẻ qua các chủ đề về bản sắc văn hóa Việt Nam đương đại.',
          bannerUrl: 'https://images.unsplash.com/photo-1544816155-12df9643f363?w=1200&auto=format&fit=crop&q=80',
          startDate: '2024-04-01',
          endDate: '2024-06-30',
          status: 'Active',
          totalEntries: 64
        },
        {
          id: 'c-2',
          title: 'Hội Thi Tranh Màu Nước & Phong Cảnh Mùa Hè 2024',
          description: 'Dành riêng cho các học viên chuyên ngành Hội họa giá vẽ với kỹ thuật thể hiện màu nước.',
          bannerUrl: 'https://images.unsplash.com/photo-1579783902614-a3fb3927b675?w=1200&auto=format&fit=crop&q=80',
          startDate: '2024-06-01',
          endDate: '2024-07-15',
          status: 'UnderJudging',
          totalEntries: 38
        }
      ]);
    });
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
          <Trophy className="w-4 h-4" /> Đấu Trường Mỹ Thuật
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Cuộc Thi & Bảng Xếp Hạng</h1>
        <p className="text-zinc-400 text-sm max-w-2xl">
          Nơi các tài năng mỹ thuật trẻ tranh tài dưới sự đánh giá công tâm theo chuẩn Rubric của Hội đồng Giám khảo.
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        {competitions.map((comp) => (
          <div key={comp.id} className="glass-card rounded-3xl overflow-hidden border border-white/10 hover:border-amber-500/30 transition-all flex flex-col group">
            <div className="relative h-56 overflow-hidden bg-zinc-900">
              <img src={comp.bannerUrl} alt={comp.title} className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500" />
              <div className="absolute top-4 left-4">
                <span className="px-3 py-1 rounded-full text-xs font-bold backdrop-blur-md border bg-emerald-500/20 text-emerald-400 border-emerald-500/30">
                  {comp.status === 'Active' ? 'Đang Nhận Bài Thi' : 'Đang Chấm Thi'}
                </span>
              </div>
            </div>
            <div className="p-8 flex-1 flex flex-col justify-between space-y-6">
              <div className="space-y-3">
                <h3 className="text-2xl font-black text-white group-hover:text-amber-400 transition-colors">{comp.title}</h3>
                <p className="text-xs text-zinc-400 leading-relaxed line-clamp-2">{comp.description}</p>
              </div>
              <div className="grid grid-cols-2 gap-4 py-4 border-y border-white/5 text-xs">
                <div className="flex items-center gap-2 text-zinc-300">
                  <Calendar className="w-4 h-4 text-amber-400" />
                  <span>Hạn chót: {comp.endDate}</span>
                </div>
                <div className="flex items-center gap-2 text-zinc-300">
                  <Award className="w-4 h-4 text-amber-400" />
                  <span>{comp.totalEntries} bài dự thi</span>
                </div>
              </div>
              <Link href={`/competitions/${comp.id}`} className="w-full text-center py-3 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-bold transition-all shadow-lg shadow-amber-500/20 flex items-center justify-center gap-2">
                Xem Bảng Xếp Hạng & Rubric <ArrowRight className="w-4 h-4" />
              </Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}