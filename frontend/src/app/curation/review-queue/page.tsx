'use client';
import React, { useState, useEffect } from 'react';
import { ShieldCheck, Check, X, Eye } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { PaintingDto } from '@/types/api';

export default function CuratorReviewQueuePage() {
  const [pendingPaintings, setPendingPaintings] = useState<PaintingDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    apiClient.get('/paintings/pending-reviews').then(res => setPendingPaintings(res.data || [])).catch(() => {
      setPendingPaintings([
        {
          id: 'p-pending-1',
          title: 'Khúc Giao Mùa',
          medium: 'Sơn dầu trên toan',
          dimensions: '90 x 140 cm',
          yearCreated: 2024,
          price: 22000000,
          isForSale: true,
          status: 'Pending',
          createdAt: '2024-06-15',
          studentId: 's-1',
          studentName: 'Nguyễn Hoàng Minh',
          studentCode: 'SV202401',
          primaryImageUrl: 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80'
        }
      ]);
    }).finally(() => setLoading(false));
  }, []);

  const handleApprove = async (id: string) => {
    setPendingPaintings(prev => prev.filter(p => p.id !== id));
    await apiClient.post(`/paintings/${id}/approve`, { notes: 'Đạt chuẩn chất lượng nghệ thuật viện' }).catch(() => {});
  };

  const handleReject = async (id: string) => {
    setPendingPaintings(prev => prev.filter(p => p.id !== id));
    await apiClient.post(`/paintings/${id}/reject`, { reason: 'Cần hoàn thiện thêm hòa sắc' }).catch(() => {});
  };

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-emerald-400 flex items-center gap-1.5">
          <ShieldCheck className="w-4 h-4" /> Hội Đồng Giám Tuyển
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Hàng Đợi Thẩm Định Tranh</h1>
        <p className="text-zinc-400 text-sm">Duyệt hoặc từ chối các tác phẩm do học viên nộp trước khi phát hành lên phòng trưng bày.</p>
      </div>

      {pendingPaintings.length === 0 ? (
        <div className="glass-card rounded-2xl p-12 text-center text-zinc-400">
          <ShieldCheck className="w-12 h-12 text-emerald-400 mx-auto mb-2" />
          <p className="font-bold text-white">Đã thẩm định xong toàn bộ tác phẩm!</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          {pendingPaintings.map((p) => (
            <div key={p.id} className="glass-card rounded-3xl p-6 border border-white/10 flex flex-col justify-between space-y-6">
              <div className="flex gap-4">
                <img src={p.primaryImageUrl} alt={p.title} className="w-32 h-32 rounded-2xl object-cover" />
                <div className="space-y-1">
                  <span className="px-2.5 py-0.5 rounded-full bg-amber-500/20 text-amber-400 text-[10px] font-bold">Chờ Thẩm Định</span>
                  <h3 className="text-lg font-bold text-white">{p.title}</h3>
                  <div className="text-xs text-zinc-400">{p.studentName} ({p.studentCode})</div>
                  <div className="text-xs text-amber-400 font-semibold">{p.medium} • {p.dimensions}</div>
                </div>
              </div>
              <div className="pt-4 border-t border-white/5 flex gap-3">
                <button onClick={() => handleApprove(p.id)} className="flex-1 py-2.5 rounded-xl bg-emerald-500 hover:bg-emerald-400 text-black text-xs font-bold flex items-center justify-center gap-1.5">
                  <Check className="w-4 h-4" /> Phê Duyệt Lên Gallery
                </button>
                <button onClick={() => handleReject(p.id)} className="py-2.5 px-4 rounded-xl bg-red-500/20 hover:bg-red-500/30 text-red-400 text-xs font-bold flex items-center justify-center gap-1.5">
                  <X className="w-4 h-4" /> Từ Chối
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}