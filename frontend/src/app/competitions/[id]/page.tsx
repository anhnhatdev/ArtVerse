'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { Trophy, Award, ArrowLeft, Sliders, Medal, Crown, Send } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { CompetitionDto, LeaderboardEntryDto } from '@/types/api';

export default function CompetitionDetailPage() {
  const params = useParams();
  const id = params?.id as string;
  const [competition, setCompetition] = useState<CompetitionDto | null>(null);
  const [leaderboard, setLeaderboard] = useState<LeaderboardEntryDto[]>([]);

  useEffect(() => {
    if (!id) return;
    apiClient.get(`/competitions/${id}`).then(res => setCompetition(res.data)).catch(() => {
      setCompetition({
        id: id,
        title: 'Cuộc Thi Sáng Tác Mỹ Thuật Trẻ Toàn Quốc 2024',
        description: 'Hội thi hội họa lớn nhất năm với tiêu chí chấm thi Rubric 100 điểm chuẩn Quốc gia.',
        bannerUrl: 'https://images.unsplash.com/photo-1544816155-12df9643f363?w=1200&auto=format&fit=crop&q=80',
        startDate: '2024-04-01',
        endDate: '2024-06-30',
        status: 'Active',
        totalEntries: 64,
        criterias: [
          { id: 'c1', name: 'Tính Sáng Tạo & Độc Đáo', maxScore: 30, weight: 30 },
          { id: 'c2', name: 'Kỹ Thuật Sử Dụng Chất Liệu', maxScore: 30, weight: 30 },
          { id: 'c3', name: 'Bố Cục & Nghệ Thuật Hòa Sắc', maxScore: 20, weight: 20 },
          { id: 'c4', name: 'Chiều Sâu Thông Điệp', maxScore: 20, weight: 20 }
        ]
      });
    });

    apiClient.get(`/competitions/${id}/leaderboard`).then(res => setLeaderboard(res.data || [])).catch(() => {
      setLeaderboard([
        {
          rank: 1,
          entryId: 'e-1',
          paintingId: 'p-1',
          paintingTitle: 'Mùa Vàng Tây Bắc',
          paintingImageUrl: 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80',
          studentName: 'Nguyễn Hoàng Minh',
          studentCode: 'SV202401',
          averageScore: 96.5,
          awardTitle: 'Giải Nhất (Gold Award)'
        },
        {
          rank: 2,
          entryId: 'e-2',
          paintingId: 'p-2',
          paintingTitle: 'Phố Cổ Sau Mưa',
          paintingImageUrl: 'https://images.unsplash.com/photo-1579783902614-a3fb3927b675?w=800&auto=format&fit=crop&q=80',
          studentName: 'Trần Thị Thu Hà',
          studentCode: 'SV202402',
          averageScore: 93.0,
          awardTitle: 'Giải Nhì (Silver Award)'
        },
        {
          rank: 3,
          entryId: 'e-3',
          paintingId: 'p-3',
          paintingTitle: 'Hồn Thiêng Đất Việt',
          paintingImageUrl: 'https://images.unsplash.com/photo-1582561424760-0321d75e81fa?w=800&auto=format&fit=crop&q=80',
          studentName: 'Lê Quốc Bảo',
          studentCode: 'SV202403',
          averageScore: 90.5,
          awardTitle: 'Giải Ba (Bronze Award)'
        }
      ]);
    });
  }, [id]);

  if (!competition) return null;

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-12">
      <Link href="/competitions" className="inline-flex items-center gap-2 text-xs font-bold text-zinc-400 hover:text-amber-400">
        <ArrowLeft className="w-4 h-4" /> Quay lại danh sách cuộc thi
      </Link>

      <div className="glass-card rounded-3xl p-8 sm:p-12 border border-white/10 space-y-6">
        <div className="flex flex-wrap items-center justify-between gap-4">
          <span className="px-3.5 py-1.5 rounded-full bg-amber-500/20 text-amber-400 border border-amber-500/30 text-xs font-bold uppercase">
            Cuộc Thi Chính Thức
          </span>
          <Link href={`/competitions/${id}/submit`} className="px-6 py-3 rounded-xl bg-amber-500 hover:bg-amber-400 text-black text-xs font-black transition-all flex items-center gap-2 shadow-lg shadow-amber-500/20">
            <Send className="w-4 h-4" /> Nộp Bài Dự Thi Của Bạn
          </Link>
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white">{competition.title}</h1>
        <p className="text-zinc-300 text-sm max-w-3xl leading-relaxed">{competition.description}</p>
      </div>

      {/* Rubric Criteria */}
      <div className="space-y-6">
        <h2 className="text-2xl font-black text-white flex items-center gap-2">
          <Sliders className="w-6 h-6 text-amber-400" /> Tiêu Chí Chấm Thi Chuẩn Rubric
        </h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {(competition.criterias || []).map((c) => (
            <div key={c.id} className="glass-card rounded-2xl p-6 border border-white/10 space-y-3">
              <div className="text-xs font-bold text-amber-400 uppercase">Trọng số {c.weight}%</div>
              <div className="text-base font-bold text-white">{c.name}</div>
              <div className="text-xs text-zinc-400 font-mono">Điểm tối đa: {c.maxScore} pts</div>
            </div>
          ))}
        </div>
      </div>

      {/* Podium Top 3 */}
      <div className="space-y-8 pt-6">
        <div className="text-center space-y-2">
          <div className="inline-flex items-center gap-2 text-xs font-bold text-amber-400 uppercase tracking-widest">
            <Trophy className="w-4 h-4" /> Bảng Vinh Danh Xuất Sắc
          </div>
          <h2 className="text-3xl font-black text-white">Podium Xếp Hạng Chung Cuộc</h2>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 items-end max-w-5xl mx-auto pt-8">
          {leaderboard[1] && (
            <div className="glass-card rounded-3xl p-6 border border-slate-400/30 text-center space-y-4 order-2 md:order-1 bg-slate-900/40">
              <div className="inline-flex p-3 rounded-2xl bg-slate-400/20 text-slate-300"><Medal className="w-8 h-8" /></div>
              <div className="text-xs font-bold text-slate-400 uppercase">Hạng 2 - Giải Nhì</div>
              <img src={leaderboard[1].paintingImageUrl} alt={leaderboard[1].paintingTitle} className="w-full h-44 object-cover rounded-2xl" />
              <div className="font-bold text-white text-base">{leaderboard[1].paintingTitle}</div>
              <div className="text-xs text-zinc-400">{leaderboard[1].studentName}</div>
              <div className="text-lg font-black text-slate-300">{leaderboard[1].averageScore} pts</div>
            </div>
          )}

          {leaderboard[0] && (
            <div className="glass-card rounded-3xl p-8 border-2 border-amber-500/50 text-center space-y-4 order-1 md:order-2 bg-amber-500/10 shadow-2xl -translate-y-4">
              <div className="inline-flex p-4 rounded-2xl bg-amber-500/30 text-amber-300 animate-bounce"><Crown className="w-10 h-10" /></div>
              <div className="text-xs font-black text-amber-400 uppercase">Quán Quân - Giải Nhất</div>
              <img src={leaderboard[0].paintingImageUrl} alt={leaderboard[0].paintingTitle} className="w-full h-52 object-cover rounded-2xl border border-amber-500/40" />
              <div className="font-black text-white text-xl">{leaderboard[0].paintingTitle}</div>
              <div className="text-xs text-amber-300 font-semibold">{leaderboard[0].studentName}</div>
              <div className="text-2xl font-black text-amber-400">{leaderboard[0].averageScore} pts</div>
            </div>
          )}

          {leaderboard[2] && (
            <div className="glass-card rounded-3xl p-6 border border-amber-700/30 text-center space-y-4 order-3 bg-amber-950/20">
              <div className="inline-flex p-3 rounded-2xl bg-amber-700/20 text-amber-600"><Medal className="w-8 h-8" /></div>
              <div className="text-xs font-bold text-amber-600 uppercase">Hạng 3 - Giải Ba</div>
              <img src={leaderboard[2].paintingImageUrl} alt={leaderboard[2].paintingTitle} className="w-full h-44 object-cover rounded-2xl" />
              <div className="font-bold text-white text-base">{leaderboard[2].paintingTitle}</div>
              <div className="text-xs text-zinc-400">{leaderboard[2].studentName}</div>
              <div className="text-lg font-black text-amber-500">{leaderboard[2].averageScore} pts</div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}