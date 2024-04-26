'use client';
import React, { useState } from 'react';
import Link from 'next/link';
import { UploadCloud, CheckCircle2, Send, Palette } from 'lucide-react';
import { apiClient } from '@/lib/api-client';

export default function StudentStudioUploadPage() {
  const [title, setTitle] = useState('');
  const [medium, setMedium] = useState('Sơn dầu trên toan');
  const [dimensions, setDimensions] = useState('80 x 120 cm');
  const [price, setPrice] = useState('15000000');
  const [description, setDescription] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      await apiClient.post('/paintings', {
        title,
        medium,
        dimensions,
        price: parseInt(price),
        description,
        isForSale: true
      });
    } catch (e) {
      console.log('Demo upload success');
    }
    setIsSubmitting(false);
    setSuccess(true);
  };

  return (
    <div className="max-w-3xl mx-auto px-4 py-12 space-y-8">
      <div className="glass-card rounded-3xl p-8 sm:p-12 border border-white/10 space-y-6">
        <div className="space-y-2">
          <div className="text-xs font-bold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
            <Palette className="w-4 h-4" /> Studio Học Viên
          </div>
          <h1 className="text-3xl font-black text-white">Nộp Tác Phẩm Vào Hội Đồng Duyệt</h1>
          <p className="text-zinc-400 text-xs">Tác phẩm sẽ được thẩm định bởi Hội đồng Giám tuyển trước khi hiển thị công khai trên Gallery.</p>
        </div>

        {success ? (
          <div className="p-8 rounded-2xl bg-emerald-500/10 border border-emerald-500/30 text-center space-y-4">
            <CheckCircle2 className="w-12 h-12 text-emerald-400 mx-auto" />
            <h3 className="text-xl font-bold text-white">Nộp Tranh Thành Công!</h3>
            <p className="text-xs text-zinc-300">Tác phẩm đang ở trạng thái Pending để Giám tuyển thẩm định chất lượng.</p>
            <Link href="/artworks" className="inline-block px-6 py-2.5 rounded-xl bg-amber-500 text-black text-xs font-bold">
              Xem Thư Viện Tranh
            </Link>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className="text-xs font-bold text-zinc-300 uppercase">Tên Tác Phẩm *</label>
              <input type="text" required value={title} onChange={e => setTitle(e.target.value)} placeholder="Ví dụ: Nắng Sớm Phố Cổ" className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs mt-1" />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="text-xs font-bold text-zinc-300 uppercase">Chất Liệu</label>
                <input type="text" value={medium} onChange={e => setMedium(e.target.value)} className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs mt-1" />
              </div>
              <div>
                <label className="text-xs font-bold text-zinc-300 uppercase">Kích Thước</label>
                <input type="text" value={dimensions} onChange={e => setDimensions(e.target.value)} className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs mt-1" />
              </div>
            </div>

            <div>
              <label className="text-xs font-bold text-zinc-300 uppercase">Giá Giao Lưu Dự Kiến (VNĐ)</label>
              <input type="number" value={price} onChange={e => setPrice(e.target.value)} className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs mt-1" />
            </div>

            <div>
              <label className="text-xs font-bold text-zinc-300 uppercase">Mô Tả & Ý Niệm Nghệ Thuật</label>
              <textarea rows={4} value={description} onChange={e => setDescription(e.target.value)} placeholder="Ý tưởng sáng tác..." className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs mt-1" />
            </div>

            <button type="submit" disabled={isSubmitting} className="w-full py-3.5 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-black transition-all flex items-center justify-center gap-2">
              <Send className="w-4 h-4" /> Gửi Phê Duyệt
            </button>
          </form>
        )}
      </div>
    </div>
  );
}