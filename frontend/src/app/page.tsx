'use client';
import React, { useEffect, useState } from 'react';
import Link from 'next/link';
import { 
  Palette, 
  Sparkles, 
  Trophy, 
  ArrowRight, 
  Eye, 
  Heart, 
  Award, 
  Layers, 
  Users, 
  CheckCircle2, 
  ShieldCheck,
  Flame
} from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { PaintingDto, ExhibitionDto, CompetitionDto } from '@/types/api';

export default function HomePage() {
  const [paintings, setPaintings] = useState<PaintingDto[]>([]);
  const [exhibition, setExhibition] = useState<ExhibitionDto | null>(null);
  const [competition, setCompetition] = useState<CompetitionDto | null>(null);

  useEffect(() => {
    // Fetch live data or fallback gracefully
    apiClient.get('/paintings?pageSize=6')
      .then(res => setPaintings(res.data.items || []))
      .catch(() => {
        setPaintings([
          {
            id: 'p-1',
            title: 'Mùa Vàng Tây Bắc',
            medium: 'Sơn dầu trên toan',
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
            medium: 'Màu nước trên giấy Arches',
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
            medium: 'Sơn mài truyền thống',
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
          }
        ]);
      });
  }, []);

  return (
    <div className="space-y-24 pb-16">
      
      {/* Hero Banner Section */}
      <section className="relative overflow-hidden pt-12 pb-20 lg:pt-20 lg:pb-32">
        <div className="absolute inset-0 -z-10 flex items-center justify-center">
          <div className="w-[600px] h-[600px] rounded-full bg-amber-500/10 blur-[140px] pointer-events-none"></div>
          <div className="w-[450px] h-[450px] rounded-full bg-purple-500/10 blur-[140px] pointer-events-none"></div>
        </div>

        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center space-y-8">
          
          <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-amber-500/10 border border-amber-500/20 text-amber-400 text-xs font-bold uppercase tracking-widest animate-pulse">
            <Sparkles className="w-3.5 h-3.5" />
            Không Gian Mỹ Thuật Học Đường Tiên Phong
          </div>

          <h1 className="text-4xl sm:text-6xl lg:text-7xl font-black tracking-tight text-white max-w-4xl mx-auto leading-tight">
            Nơi Tinh Hoa Hội Họa <br />
            <span className="gold-gradient-text">Tỏa Sáng Khát Vọng Trẻ</span>
          </h1>

          <p className="text-base sm:text-lg text-zinc-400 max-w-2xl mx-auto leading-relaxed font-normal">
            Nền tảng số quản trị học thuật, tổ chức triển lãm thực tế ảo và hội đồng chấm thi theo chuẩn Rubric quốc tế cho trường mỹ thuật.
          </p>

          <div className="flex flex-wrap items-center justify-center gap-4 pt-4">
            <Link
              href="/artworks"
              className="px-8 py-4 rounded-xl text-sm font-bold bg-gradient-to-r from-amber-400 via-amber-500 to-amber-600 text-black hover:scale-105 transition-all shadow-xl shadow-amber-500/25 flex items-center gap-2"
            >
              <Palette className="w-4 h-4" />
              Khám Phá Phòng Trưng Bày
              <ArrowRight className="w-4 h-4" />
            </Link>

            <Link
              href="/competitions"
              className="px-8 py-4 rounded-xl text-sm font-bold glass-card text-white hover:bg-white/10 hover:border-white/20 transition-all flex items-center gap-2"
            >
              <Trophy className="w-4 h-4 text-amber-400" />
              Xem Cuộc Thi & Xếp Hạng
            </Link>
          </div>

          {/* Key Metrics Counter Bar */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4 max-w-4xl mx-auto pt-12">
            {[
              { label: 'Tác Phẩm Giám Tuyển', value: '450+', icon: Palette },
              { label: 'Họa Sĩ Học Viên', value: '120+', icon: Users },
              { label: 'Cuộc Thi Đã Tổ Chức', value: '18+', icon: Trophy },
              { label: 'Lượt Thưởng Lãm', value: '25,000+', icon: Eye },
            ].map((stat, i) => (
              <div key={i} className="glass-card rounded-2xl p-5 text-center border border-white/5">
                <div className="text-2xl sm:text-3xl font-black text-white">{stat.value}</div>
                <div className="text-xs text-zinc-400 font-medium mt-1">{stat.label}</div>
              </div>
            ))}
          </div>

        </div>
      </section>

      {/* Featured Artworks Masonry Showcase */}
      <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-10">
        <div className="flex flex-col md:flex-row items-start md:items-end justify-between gap-4 border-b border-white/10 pb-6">
          <div>
            <div className="text-amber-400 text-xs font-bold uppercase tracking-widest flex items-center gap-1.5 mb-2">
              <Flame className="w-4 h-4" />
              Bộ Sưu Tập Tuyển Chọn
            </div>
            <h2 className="text-3xl font-black text-white tracking-tight">Tác Phẩm Mỹ Thuật Tiêu Biểu</h2>
          </div>
          <Link href="/artworks" className="text-sm font-bold text-amber-400 hover:text-amber-300 flex items-center gap-1">
            Xem toàn bộ 450+ tranh <ArrowRight className="w-4 h-4" />
          </Link>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {paintings.map((painting) => (
            <div 
              key={painting.id} 
              className="glass-card rounded-2xl overflow-hidden group border border-white/10 hover:border-amber-500/30 transition-all flex flex-col"
            >
              <div className="relative aspect-[4/3] overflow-hidden bg-zinc-900">
                <img
                  src={painting.primaryImageUrl || 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80'}
                  alt={painting.title}
                  className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
                />
                <div className="absolute top-3 left-3 px-3 py-1 rounded-full bg-black/60 backdrop-blur-md text-[11px] font-bold text-amber-400 border border-white/10">
                  {painting.medium}
                </div>
                <div className="absolute top-3 right-3 px-2.5 py-1 rounded-full bg-emerald-500/20 backdrop-blur-md text-[10px] font-bold text-emerald-400 border border-emerald-500/30">
                  Đã Duyệt
                </div>
              </div>

              <div className="p-6 flex-1 flex flex-col justify-between space-y-4">
                <div>
                  <h3 className="text-xl font-bold text-white group-hover:text-amber-400 transition-colors">
                    {painting.title}
                  </h3>
                  <p className="text-xs text-zinc-400 mt-1">
                    Tác giả: <span className="text-zinc-200 font-semibold">{painting.studentName}</span> ({painting.studentCode})
                  </p>
                </div>

                <div className="pt-4 border-t border-white/5 flex items-center justify-between">
                  <div>
                    <div className="text-[10px] text-zinc-500 uppercase font-bold">Kích Thước</div>
                    <div className="text-xs font-semibold text-zinc-300">{painting.dimensions}</div>
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
      </section>

      {/* Virtual Exhibition Highlight Card */}
      <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="relative rounded-3xl overflow-hidden glass-card border border-white/10 p-8 sm:p-12 lg:p-16">
          <div className="absolute top-0 right-0 w-96 h-96 bg-amber-500/10 rounded-full blur-3xl -z-10"></div>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-10 items-center">
            <div className="space-y-6">
              <div className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-lg bg-emerald-500/10 text-emerald-400 text-xs font-bold uppercase tracking-wider border border-emerald-500/20">
                <Sparkles className="w-4 h-4" /> Đang Diễn Ra Trực Tuyến
              </div>
              <h2 className="text-3xl sm:text-4xl font-black text-white tracking-tight">
                Triển Lãm Số: <br />
                <span className="gold-gradient-text">"Sắc Màu Di Sản 2024"</span>
              </h2>
              <p className="text-sm text-zinc-300 leading-relaxed">
                Quy tụ 45 tác phẩm xuất sắc nhất từ các tài năng hội họa trẻ. Thưởng lãm trực tuyến, tương tác thả tim realtime và nhận xét cùng giám tuyển viện mỹ thuật.
              </p>
              <div className="flex flex-wrap gap-4 pt-2">
                <Link
                  href="/exhibitions"
                  className="px-6 py-3.5 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-extrabold transition-all shadow-lg shadow-amber-500/20 flex items-center gap-2"
                >
                  Bước Vào Phòng Triển Lãm <ArrowRight className="w-4 h-4" />
                </Link>
              </div>
            </div>

            <div className="relative rounded-2xl overflow-hidden shadow-2xl border border-white/10">
              <img
                src="https://images.unsplash.com/photo-1544816155-12df9643f363?w=800&auto=format&fit=crop&q=80"
                alt="Triển lãm ArtVerse"
                className="w-full h-80 object-cover"
              />
              <div className="absolute bottom-0 inset-x-0 p-4 bg-gradient-to-t from-black/90 to-transparent flex items-center justify-between text-xs">
                <span className="text-white font-bold">Giám tuyển: GS. TS. Nguyễn Đình Văn</span>
                <span className="text-amber-400 font-bold">45 Tác phẩm</span>
              </div>
            </div>
          </div>
        </div>
      </section>

    </div>
  );
}