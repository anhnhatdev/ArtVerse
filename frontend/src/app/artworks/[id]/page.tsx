'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { 
  Palette, 
  ShieldCheck, 
  ArrowLeft, 
  Calendar, 
  Maximize2, 
  Tag, 
  Share2, 
  Heart,
  CheckCircle,
  User
} from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { PaintingDto } from '@/types/api';

export default function ArtworkDetailPage() {
  const params = useParams();
  const id = params?.id as string;
  const [painting, setPainting] = useState<PaintingDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [liked, setLiked] = useState(false);

  useEffect(() => {
    if (!id) return;
    setLoading(true);
    apiClient.get(`/paintings/${id}`)
      .then(res => setPainting(res.data))
      .catch(() => {
        setPainting({
          id: id,
          title: 'Mùa Vàng Tây Bắc',
          description: 'Tác phẩm khắc họa vẻ đẹp hùng vĩ của ruộng bậc thang Mù Cang Chải vào mùa lúa chín vàng óng ả. Sự tương phản giữa ánh nắng hoàng hôn và những rặng núi mờ sương tạo nên chiều sâu cảm xúc sâu lắng.',
          medium: 'Sơn dầu trên toan (Canvas)',
          dimensions: '80 x 120 cm',
          yearCreated: 2024,
          price: 15000000,
          isForSale: true,
          status: 'Approved',
          curatorNotes: 'Bố cục vững chãi, kỹ thuật sử dụng bay điêu luyện với các lớp sơn dày tạo khối nổi sống động.',
          createdAt: '2024-05-10',
          studentId: 's-1',
          studentName: 'Nguyễn Hoàng Minh',
          studentCode: 'SV202401',
          primaryImageUrl: 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=1200&auto=format&fit=crop&q=80'
        });
      })
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <div className="py-24 text-center text-zinc-500 text-sm">Đang tải thông tin tác phẩm...</div>;
  }

  if (!painting) {
    return (
      <div className="max-w-4xl mx-auto py-24 px-4 text-center">
        <h2 className="text-2xl font-bold text-white">Không tìm thấy tác phẩm</h2>
        <Link href="/artworks" className="mt-4 inline-block text-amber-400 underline text-sm">Quay lại thư viện tranh</Link>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-12">
      
      {/* Navigation breadcrumb */}
      <Link href="/artworks" className="inline-flex items-center gap-2 text-xs font-bold text-zinc-400 hover:text-amber-400 transition-colors">
        <ArrowLeft className="w-4 h-4" /> Quay lại thư viện tranh
      </Link>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-12">
        
        {/* Left: HD Artwork Preview */}
        <div className="lg:col-span-7 space-y-4">
          <div className="glass-card rounded-3xl overflow-hidden border border-white/10 p-2 sm:p-4 bg-zinc-950/80 shadow-2xl">
            <img
              src={painting.primaryImageUrl}
              alt={painting.title}
              className="w-full rounded-2xl object-cover max-h-[600px]"
            />
          </div>
          <div className="flex items-center justify-between text-xs text-zinc-400 px-2">
            <span>Độ phân giải HD gốc • Giám định bảo mật</span>
            <button 
              onClick={() => setLiked(!liked)}
              className={`flex items-center gap-1.5 px-3 py-1.5 rounded-lg border transition-all ${
                liked ? 'bg-rose-500/20 text-rose-400 border-rose-500/30' : 'bg-white/5 text-zinc-300 border-white/10'
              }`}
            >
              <Heart className={`w-4 h-4 ${liked ? 'fill-current' : ''}`} />
              <span>{liked ? 'Đã yêu thích' : 'Yêu thích'}</span>
            </button>
          </div>
        </div>

        {/* Right: Specifications, Artist & Curation Verification */}
        <div className="lg:col-span-5 space-y-8">
          
          <div className="space-y-3">
            <div className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-emerald-500/10 text-emerald-400 text-xs font-bold border border-emerald-500/20">
              <ShieldCheck className="w-3.5 h-3.5" /> Đã Giám Tuyển Bởi Hội Đồng
            </div>
            <h1 className="text-3xl sm:text-4xl font-black text-white tracking-tight">{painting.title}</h1>
            <p className="text-xs text-zinc-400">
              Sáng tác năm <strong className="text-zinc-200">{painting.yearCreated}</strong> • Mã tác phẩm: <span className="font-mono text-amber-400">{painting.id}</span>
            </p>
          </div>

          {/* Price & Acquisition Box */}
          <div className="glass-card rounded-2xl p-6 border border-white/10 space-y-4 bg-gradient-to-br from-white/5 to-white/0">
            <div className="flex items-center justify-between">
              <div>
                <span className="text-[11px] text-zinc-400 uppercase font-bold">Giá Niêm Yết</span>
                <div className="text-2xl font-black text-amber-400">
                  {painting.price ? `${painting.price.toLocaleString('vi-VN')} VNĐ` : 'Trưng bày lưu niệm'}
                </div>
              </div>
              <div className="text-xs px-3 py-1 rounded-lg bg-white/10 text-zinc-300 font-semibold">
                {painting.isForSale ? 'Đang mở giao lưu' : 'Lưu trữ viện'}
              </div>
            </div>
          </div>

          {/* Artist Card */}
          <div className="glass-card rounded-2xl p-5 border border-white/10 flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="w-12 h-12 rounded-xl bg-amber-500/20 border border-amber-500/30 flex items-center justify-center font-black text-amber-400 text-lg">
                {painting.studentName?.charAt(0) || 'H'}
              </div>
              <div>
                <div className="text-sm font-bold text-white">{painting.studentName}</div>
                <div className="text-xs text-zinc-400">Mã học viên: {painting.studentCode}</div>
              </div>
            </div>
            <Link
              href={`/students/${painting.studentId}`}
              className="px-3 py-1.5 rounded-lg bg-white/5 hover:bg-white/10 text-xs font-bold text-amber-400 transition-colors"
            >
              Hồ sơ họa sĩ
            </Link>
          </div>

          {/* Technical Specs List */}
          <div className="space-y-3 pt-2">
            <h4 className="text-xs font-bold uppercase tracking-wider text-zinc-400">Thông Số Kỹ Thuật</h4>
            <div className="grid grid-cols-2 gap-3 text-xs">
              <div className="p-3 rounded-xl bg-white/5 border border-white/5">
                <span className="text-zinc-500 block">Chất liệu</span>
                <span className="font-semibold text-zinc-200">{painting.medium}</span>
              </div>
              <div className="p-3 rounded-xl bg-white/5 border border-white/5">
                <span className="text-zinc-500 block">Kích thước</span>
                <span className="font-semibold text-zinc-200">{painting.dimensions}</span>
              </div>
            </div>
          </div>

          {/* Description & Curator Notes */}
          <div className="space-y-3 pt-2">
            <h4 className="text-xs font-bold uppercase tracking-wider text-zinc-400">Lời Bình Giám Tuyển</h4>
            <div className="p-4 rounded-xl bg-amber-500/5 border border-amber-500/15 text-xs text-zinc-300 leading-relaxed italic">
              "{painting.curatorNotes || painting.description}"
            </div>
          </div>

        </div>

      </div>

    </div>
  );
}