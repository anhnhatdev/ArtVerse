'use client';
import React, { useState } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { Sliders, ArrowLeft, CheckCircle2 } from 'lucide-react';
import { apiClient } from '@/lib/api-client';

export default function JudgingRoomPage() {
  const params = useParams();
  const id = params?.id as string;

  const [scores, setScores] = useState<Record<string, number>>({
    c1: 28,
    c2: 27,
    c3: 18,
    c4: 19
  });
  const [feedback, setFeedback] = useState('Tác phẩm xử lý ánh sáng rất tốt, nhịp điệu màu sắc phong phú.');
  const [submitted, setSubmitted] = useState(false);

  const totalScore = Object.values(scores).reduce((a, b) => a + b, 0);

  const handleSubmitScore = async () => {
    setSubmitted(true);
    await apiClient.post(`/competitions/entries/${id}/score`, { scores, feedback }).catch(() => {});
  };

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 space-y-6">
      <Link href="/competitions" className="inline-flex items-center gap-2 text-xs font-bold text-zinc-400 hover:text-amber-400">
        <ArrowLeft className="w-4 h-4" /> Quay lại danh sách
      </Link>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-8 items-start">
        {/* Left: HD Artwork Viewer */}
        <div className="lg:col-span-7 glass-card rounded-3xl p-4 border border-white/10 bg-zinc-950">
          <img src="https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=1200&auto=format&fit=crop&q=80" alt="Judging HD" className="w-full rounded-2xl object-cover max-h-[550px]" />
          <div className="p-4 space-y-1">
            <h2 className="text-xl font-bold text-white">Mùa Vàng Tây Bắc</h2>
            <p className="text-xs text-zinc-400">Thí sinh: Nguyễn Hoàng Minh (SV202401) • Sơn dầu trên toan 80x120cm</p>
          </div>
        </div>

        {/* Right: Rubric Scoring Sliders */}
        <div className="lg:col-span-5 glass-card rounded-3xl p-6 sm:p-8 border border-white/10 space-y-6">
          <div className="flex items-center justify-between border-b border-white/10 pb-4">
            <h3 className="text-lg font-bold text-white flex items-center gap-2">
              <Sliders className="w-5 h-5 text-amber-400" /> Bảng Điểm Rubric
            </h3>
            <div className="text-xl font-black text-amber-400">{totalScore} / 100 pts</div>
          </div>

          <div className="space-y-4">
            {[
              { id: 'c1', label: '1. Sáng Tạo & Độc Đáo', max: 30 },
              { id: 'c2', label: '2. Kỹ Thuật Chất Liệu', max: 30 },
              { id: 'c3', label: '3. Bố Cục & Hòa Sắc', max: 20 },
              { id: 'c4', label: '4. Chiều Sâu Thông Điệp', max: 20 },
            ].map(c => (
              <div key={c.id} className="space-y-1.5">
                <div className="flex justify-between text-xs font-bold text-zinc-300">
                  <span>{c.label}</span>
                  <span className="text-amber-400">{scores[c.id]} / {c.max} pts</span>
                </div>
                <input
                  type="range"
                  min={0}
                  max={c.max}
                  value={scores[c.id]}
                  onChange={e => setScores({ ...scores, [c.id]: parseInt(e.target.value) })}
                  className="w-full accent-amber-400"
                />
              </div>
            ))}
          </div>

          <div className="space-y-2">
            <label className="text-xs font-bold text-zinc-300">Nhận Xét Của Giám Khảo</label>
            <textarea rows={3} value={feedback} onChange={e => setFeedback(e.target.value)} className="w-full p-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs" />
          </div>

          <button onClick={handleSubmitScore} className="w-full py-3.5 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-black transition-all">
            {submitted ? '✓ Đã Lưu Điểm Số' : 'Xác Nhận Điểm Số Giám Khảo'}
          </button>
        </div>
      </div>
    </div>
  );
}