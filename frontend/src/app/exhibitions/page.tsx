'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { Sparkles, ArrowRight } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { ExhibitionDto } from '@/types/api';

export default function ExhibitionsPage() {
  const [exhibitions, setExhibitions] = useState<ExhibitionDto[]>([]);
  useEffect(() => {
    apiClient.get('/exhibitions').then(res => setExhibitions(res.data || [])).catch(() => {
      setExhibitions([
        {
          id: 'ex-1',
          title: 'Triển Lãm Sắc Màu Di Sản 2024',
          theme: 'Bản sắc Việt Nam qua góc nhìn thế hệ mới',
          description: 'Tuyển tập 45 tác phẩm xuất sắc nhất phản ánh văn hóa truyền thống.',
          bannerUrl: 'https://images.unsplash.com/photo-1544816155-12df9643f363?w=1200&auto=format&fit=crop&q=80',
          startDate: '2024-05-01',
          endDate: '2024-08-30',
          status: 'Ongoing',
          curatorName: 'GS. TS. Nguyễn Đình Văn',
          totalArtworks: 45
        }
      ]);
    });
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
          <Sparkles className="w-4 h-4" /> Triển Lãm Không Gian Số
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Triển Lãm Mỹ Thuật Trực Tuyến</h1>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        {exhibitions.map((ex) => (
          <div key={ex.id} className="glass-card rounded-3xl overflow-hidden border border-white/10 hover:border-amber-500/30 transition-all flex flex-col group">
            <img src={ex.bannerUrl} alt={ex.title} className="w-full h-64 object-cover" />
            <div className="p-8 space-y-4">
              <h3 className="text-2xl font-black text-white">{ex.title}</h3>
              <p className="text-xs text-zinc-400">{ex.description}</p>
              <div className="pt-4 border-t border-white/5 flex items-center justify-between text-xs">
                <span>Giám tuyển: <strong className="text-white">{ex.curatorName}</strong></span>
                <span className="text-amber-400 font-bold">{ex.totalArtworks} Tác phẩm</span>
              </div>
              <Link href={`/exhibitions/${ex.id}`} className="w-full text-center py-3 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-bold transition-all flex items-center justify-center gap-2">
                Bước Vào Phòng Triển Lãm <ArrowRight className="w-4 h-4" />
              </Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}