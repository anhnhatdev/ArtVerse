'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { Users, Palette, ArrowRight } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { StudentDto } from '@/types/api';

export default function StudentsDirectoryPage() {
  const [students, setStudents] = useState<StudentDto[]>([]);
  useEffect(() => {
    apiClient.get('/students').then(res => setStudents(res.data || [])).catch(() => {
      setStudents([
        {
          id: 's-1',
          fullName: 'Nguyễn Hoàng Minh',
          code: 'SV202401',
          email: 'minh.nh@artverse.edu.vn',
          major: 'Hội Họa Sơn Dầu & Giá Vẽ',
          enrollmentYear: 2021,
          totalArtworks: 12,
          bio: 'Đam mê phong cảnh vùng cao Tây Bắc và kỹ thuật đắp màu bay nổi biểu hiện cảm xúc.'
        },
        {
          id: 's-2',
          fullName: 'Trần Thị Thu Hà',
          code: 'SV202402',
          email: 'ha.ttt@artverse.edu.vn',
          major: 'Mỹ Thuật Truyền Thống & Tranh Lụa',
          enrollmentYear: 2022,
          totalArtworks: 8,
          bio: 'Nghiên cứu kỹ thuật nhuộm màu trên lụa tơ tằm truyền thống Hà Đông.'
        }
      ]);
    });
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
          <Users className="w-4 h-4" /> Danh Bạ Học Viện
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Họa Sĩ Trẻ Tiêu Biểu</h1>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {students.map((st) => (
          <div key={st.id} className="glass-card rounded-3xl p-8 border border-white/10 flex flex-col justify-between space-y-6">
            <div className="space-y-3">
              <div className="w-14 h-14 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 flex items-center justify-center font-black text-black text-xl">
                {st.fullName.charAt(0)}
              </div>
              <h3 className="text-xl font-bold text-white">{st.fullName}</h3>
              <div className="text-xs text-amber-400 font-semibold">{st.major}</div>
              <p className="text-xs text-zinc-400">"{st.bio}"</p>
            </div>
            <div className="pt-4 border-t border-white/5 flex items-center justify-between">
              <span className="text-xs text-zinc-300">{st.totalArtworks} tác phẩm</span>
              <Link href={`/students/${st.id}`} className="px-4 py-2 rounded-xl bg-amber-500 text-black text-xs font-bold flex items-center gap-1">
                Xem Hồ Sơ <ArrowRight className="w-3.5 h-3.5" />
              </Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}