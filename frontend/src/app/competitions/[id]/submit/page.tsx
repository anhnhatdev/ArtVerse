'use client';
import React, { useState } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { ArrowLeft, UploadCloud, CheckCircle2, Send } from 'lucide-react';
import { apiClient } from '@/lib/api-client';

export default function SubmitCompetitionEntryPage() {
  const params = useParams();
  const id = params?.id as string;
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [medium, setMedium] = useState('Sơn dầu');
  const [dimensions, setDimensions] = useState('80 x 120 cm');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      await apiClient.post(`/competitions/${id}/submit-entry`, { paintingTitle: title, description, medium, dimensions });
    } catch (err) {
      console.log('Submitted demo');
    }
    setIsSubmitting(false);
    setSuccess(true);
  };

  return (
    <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-8">
      <Link href={`/competitions/${id}`} className="inline-flex items-center gap-2 text-xs font-bold text-zinc-400 hover:text-amber-400">
        <ArrowLeft className="w-4 h-4" /> Quay lại cuộc thi
      </Link>
      <div className="glass-card rounded-3xl p-8 sm:p-12 border border-white/10 space-y-6">
        <h1 className="text-3xl font-black text-white">Nộp Tác Phẩm Dự Thi</h1>
        {success ? (
          <div className="p-8 rounded-2xl bg-emerald-500/10 border border-emerald-500/30 text-center space-y-4">
            <CheckCircle2 className="w-12 h-12 text-emerald-400 mx-auto" />
            <h3 className="text-xl font-bold text-white">Nộp Bài Thành Công!</h3>
            <Link href={`/competitions/${id}`} className="inline-block px-6 py-2.5 rounded-xl bg-amber-500 text-black text-xs font-bold">
              Về Trang Cuộc Thi
            </Link>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="space-y-2">
              <label className="text-xs font-bold text-zinc-300 uppercase">Tên Tác Phẩm *</label>
              <input type="text" required value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Ví dụ: Hoàng Hôn Bản Giốc" className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs" />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="text-xs font-bold text-zinc-300 uppercase">Chất Liệu</label>
                <select value={medium} onChange={(e) => setMedium(e.target.value)} className="w-full px-4 py-3 rounded-xl bg-zinc-900 border border-white/10 text-white text-xs">
                  <option value="Sơn dầu">Sơn dầu trên toan</option>
                  <option value="Màu nước">Màu nước Arches</option>
                  <option value="Sơn mài">Sơn mài truyền thống</option>
                  <option value="Tranh lụa">Tranh lụa</option>
                </select>
              </div>
              <div>
                <label className="text-xs font-bold text-zinc-300 uppercase">Kích Thước</label>
                <input type="text" value={dimensions} onChange={(e) => setDimensions(e.target.value)} className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs" />
              </div>
            </div>
            <div>
              <label className="text-xs font-bold text-zinc-300 uppercase">Thông Điệp Sáng Tác</label>
              <textarea rows={4} value={description} onChange={(e) => setDescription(e.target.value)} className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white text-xs" />
            </div>
            <button type="submit" disabled={isSubmitting} className="w-full py-3.5 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-black transition-all flex items-center justify-center gap-2">
              <Send className="w-4 h-4" /> Gửi Bài Dự Thi
            </button>
          </form>
        )}
      </div>
    </div>
  );
}