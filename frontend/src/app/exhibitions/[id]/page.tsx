'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { ArrowLeft, Heart } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { ExhibitionDto } from '@/types/api';

export default function ExhibitionDetailPage() {
  const params = useParams();
  const id = params?.id as string;
  const [exhibition, setExhibition] = useState<ExhibitionDto | null>(null);
  const [likes, setLikes] = useState<Record<string, number>>({});

  useEffect(() => {
    if (!id) return;
    apiClient.get(`/exhibitions/${id}`).then(res => setExhibition(res.data)).catch(() => {
      setExhibition({
        id: id,
        title: 'Triển Lãm Sắc Màu Di Sản 2024',
        theme: 'Bản sắc Việt Nam',
        description: 'Không gian triển lãm thực tế ảo hội tụ những tác phẩm tranh sáng tác tiêu biểu nhất.',
        bannerUrl: 'https://images.unsplash.com/photo-1544816155-12df9643f363?w=1200&auto=format&fit=crop&q=80',
        startDate: '2024-05-01',
        endDate: '2024-08-30',
        status: 'Ongoing',
        curatorName: 'GS. TS. Nguyễn Đình Văn',
        totalArtworks: 3,
        artworks: [
          {
            id: 'art-1',
            paintingId: 'p-1',
            title: 'Mùa Vàng Tây Bắc',
            studentName: 'Nguyễn Hoàng Minh',
            imageUrl: 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80',
            medium: 'Sơn dầu trên toan',
            likeCount: 142
          },
          {
            id: 'art-2',
            paintingId: 'p-2',
            title: 'Phố Cổ Sau Mưa',
            studentName: 'Trần Thị Thu Hà',
            imageUrl: 'https://images.unsplash.com/photo-1579783902614-a3fb3927b675?w=800&auto=format&fit=crop&q=80',
            medium: 'Màu nước Arches',
            likeCount: 98
          }
        ]
      });
    });
  }, [id]);

  const handleLike = (artId: string, currentCount: number) => {
    const next = (likes[artId] ?? currentCount) + 1;
    setLikes(prev => ({ ...prev, [artId]: next }));
    apiClient.post(`/exhibitions/artworks/${artId}/like`).catch(() => {});
  };

  if (!exhibition) return null;

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <Link href="/exhibitions" className="inline-flex items-center gap-2 text-xs font-bold text-zinc-400 hover:text-amber-400">
        <ArrowLeft className="w-4 h-4" /> Quay lại triển lãm
      </Link>
      <div className="glass-card rounded-3xl p-8 sm:p-12 border border-white/10 space-y-4">
        <h1 className="text-3xl sm:text-5xl font-black text-white">{exhibition.title}</h1>
        <p className="text-zinc-300 text-sm max-w-3xl leading-relaxed">{exhibition.description}</p>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {(exhibition.artworks || []).map((art) => (
          <div key={art.id} className="glass-card rounded-2xl overflow-hidden border border-white/10 flex flex-col justify-between">
            <img src={art.imageUrl} alt={art.title} className="w-full aspect-[4/3] object-cover" />
            <div className="p-6 space-y-4">
              <h3 className="text-lg font-bold text-white">{art.title}</h3>
              <div className="flex items-center justify-between">
                <button onClick={() => handleLike(art.id, art.likeCount)} className="flex items-center gap-1.5 px-3 py-1.5 rounded-lg border border-white/10 text-xs font-bold text-rose-400">
                  <Heart className="w-4 h-4 fill-current" /> {likes[art.id] ?? art.likeCount}
                </button>
                <Link href={`/artworks/${art.paintingId}`} className="px-3 py-1.5 rounded-lg bg-amber-500 text-black text-xs font-bold">
                  Chi Tiết
                </Link>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}