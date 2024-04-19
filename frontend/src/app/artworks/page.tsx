'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { Palette, Search, Filter, Eye, ArrowUpDown, Sparkles } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { PaintingDto } from '@/types/api';

export default function ArtworksGalleryPage() {
  const [paintings, setPaintings] = useState<PaintingDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [selectedMedium, setSelectedMedium] = useState('All');

  const mediums = ['All', 'Sơn dầu', 'Màu nước', 'Sơn mài', 'Tranh lụa', 'Kỹ thuật số'];

  useEffect(() => {
    setLoading(true);
    apiClient.get('/paintings?pageSize=20')
      .then(res => setPaintings(res.data.items || []))
      .catch(() => {
        setPaintings([
          {
            id: 'p-1',
            title: 'Mùa Vàng Tây Bắc',
            medium: 'Sơn dầu',
            dimensions: '80 x 120 cm',
            yearCreated: 2024,
            price: 15000000,
            isForSale: true,
            status: 'Approved',
            createdAt: '2024-05-10',
            studentId: 's-1',
            studentName: 'Nguyễn Hoàng Minh',
            studentCode: 'SV202401',
            primaryImageUrl: 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80'
          },
          {
            id: 'p-2',
            title: 'Phố Cổ Sau Mưa',
            medium: 'Màu nước',
            dimensions: '50 x 70 cm',
            yearCreated: 2024,
            price: 8500000,
            isForSale: true,
            status: 'Approved',
            createdAt: '2024-05-12',
            studentId: 's-2',
            studentName: 'Trần Thị Thu Hà',
            studentCode: 'SV202402',
            primaryImageUrl: 'https://images.unsplash.com/photo-1579783902614-a3fb3927b675?w=800&auto=format&fit=crop&q=80'
          },
          {
            id: 'p-3',
            title: 'Hồn Thiêng Đất Việt',
            medium: 'Sơn mài',
            dimensions: '100 x 150 cm',
            yearCreated: 2024,
            price: 32000000,
            isForSale: false,
            status: 'Approved',
            createdAt: '2024-05-15',
            studentId: 's-3',
            studentName: 'Lê Quốc Bảo',
            studentCode: 'SV202403',
            primaryImageUrl: 'https://images.unsplash.com/photo-1582561424760-0321d75e81fa?w=800&auto=format&fit=crop&q=80'
          },
          {
            id: 'p-4',
            title: 'Nét Duyên Kinh Kỳ',
            medium: 'Tranh lụa',
            dimensions: '60 x 90 cm',
            yearCreated: 2024,
            price: 18000000,
            isForSale: true,
            status: 'Approved',
            createdAt: '2024-05-18',
            studentId: 's-4',
            studentName: 'Phạm Hồng Nhung',
            studentCode: 'SV202404',
            primaryImageUrl: 'https://images.unsplash.com/photo-1579783902614-a3fb3927b675?w=800&auto=format&fit=crop&q=80'
          }
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const filtered = paintings.filter(p => {
    const matchSearch = p.title.toLowerCase().includes(search.toLowerCase()) || 
                        p.studentName.toLowerCase().includes(search.toLowerCase());
    const matchMedium = selectedMedium === 'All' || p.medium?.includes(selectedMedium);
    return matchSearch && matchMedium;
  });

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      
      {/* Header */}
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
          <Palette className="w-4 h-4" /> Tuyển Tập Mỹ Thuật
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Phòng Trưng Bày Tác Phẩm</h1>
        <p className="text-zinc-400 text-sm max-w-2xl">
          Chiêm ngưỡng các tác phẩm tranh sơn dầu, màu nước, sơn mài và tranh lụa được giám định bởi Hội đồng nghệ thuật viện.
        </p>
      </div>

      {/* Filter & Search Toolbar */}
      <div className="glass-card rounded-2xl p-4 sm:p-6 border border-white/10 space-y-4">
        <div className="flex flex-col sm:flex-row gap-4 justify-between items-center">
          
          {/* Search Box */}
          <div className="relative w-full sm:w-80">
            <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-400" />
            <input
              type="text"
              placeholder="Tìm kiếm tác phẩm, họa sĩ..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-10 pr-4 py-2.5 rounded-xl bg-white/5 border border-white/10 text-white placeholder-zinc-500 text-xs focus:outline-none focus:border-amber-500/50"
            />
          </div>

          {/* Medium Filter Pills */}
          <div className="flex flex-wrap gap-2 w-full sm:w-auto justify-start sm:justify-end">
            {mediums.map(m => (
              <button
                key={m}
                onClick={() => setSelectedMedium(m)}
                className={`px-3 py-1.5 rounded-lg text-xs font-bold transition-all ${
                  selectedMedium === m 
                    ? 'bg-amber-500 text-black shadow-lg shadow-amber-500/20' 
                    : 'bg-white/5 text-zinc-400 hover:text-white hover:bg-white/10'
                }`}
              >
                {m === 'All' ? 'Tất Cả Chất Liệu' : m}
              </button>
            ))}
          </div>

        </div>
      </div>

      {/* Grid of Artworks */}
      {loading ? (
        <div className="py-24 text-center text-zinc-500 text-sm">Đang tải danh mục tranh...</div>
      ) : filtered.length === 0 ? (
        <div className="glass-card rounded-2xl p-12 text-center text-zinc-400 space-y-2">
          <p className="font-bold text-white">Không tìm thấy tác phẩm phù hợp</p>
          <p className="text-xs">Hãy thử thay đổi từ khóa tìm kiếm hoặc chọn chất liệu khác.</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {filtered.map((painting) => (
            <div
              key={painting.id}
              className="glass-card rounded-2xl overflow-hidden group border border-white/10 hover:border-amber-500/30 transition-all flex flex-col"
            >
              <div className="relative aspect-[4/3] overflow-hidden bg-zinc-900">
                <img
                  src={painting.primaryImageUrl || 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80'}
                  alt={painting.title}
                  className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                />
                <div className="absolute top-3 left-3 px-3 py-1 rounded-full bg-black/70 backdrop-blur-md text-[11px] font-bold text-amber-400 border border-white/10">
                  {painting.medium}
                </div>
              </div>

              <div className="p-6 flex-1 flex flex-col justify-between space-y-4">
                <div>
                  <h3 className="text-lg font-bold text-white group-hover:text-amber-400 transition-colors">
                    {painting.title}
                  </h3>
                  <p className="text-xs text-zinc-400 mt-1">
                    Họa sĩ: <span className="text-zinc-200 font-semibold">{painting.studentName}</span>
                  </p>
                </div>

                <div className="pt-4 border-t border-white/5 flex items-center justify-between">
                  <div className="text-xs font-bold text-amber-400">
                    {painting.price ? `${painting.price.toLocaleString('vi-VN')} đ` : 'Trưng bày'}
                  </div>
                  <Link
                    href={`/artworks/${painting.id}`}
                    className="px-4 py-2 rounded-lg bg-white/5 hover:bg-amber-500 hover:text-black text-xs font-bold text-white transition-all flex items-center gap-1.5"
                  >
                    <Eye className="w-3.5 h-3.5" /> Chi Tiết
                  </Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

    </div>
  );
}