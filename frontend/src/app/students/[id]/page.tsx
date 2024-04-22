'use client';
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { ArrowLeft } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { StudentDto } from '@/types/api';

export default function StudentProfilePage() {
  const params = useParams();
  const id = params?.id as string;
  const [student, setStudent] = useState<StudentDto | null>(null);

  useEffect(() => {
    if (!id) return;
    apiClient.get(`/students/${id}`).then(res => setStudent(res.data)).catch(() => {
      setStudent({
        id: id,
        fullName: 'Nguyễn Hoàng Minh',
        code: 'SV202401',
        email: 'minh.nh@artverse.edu.vn',
        major: 'Hội Họa Sơn Dầu & Giá Vẽ',
        enrollmentYear: 2021,
        totalArtworks: 1,
        bio: 'Sinh viên tài năng đạt nhiều giải thưởng xuất sắc tại các cuộc thi mỹ thuật toàn quốc.',
        paintings: [
          {
            id: 'p-1',
            title: 'Mùa Vàng Tây Bắc',
            medium: 'Sơn dầu',
            dimensions: '80 x 120 cm',
            yearCreated: 2024,
            isForSale: true,
            status: 'Approved',
            createdAt: '2024-05-10',
            studentId: id,
            studentName: 'Nguyễn Hoàng Minh',
            studentCode: 'SV202401',
            primaryImageUrl: 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop&q=80'
          }
        ]
      });
    });
  }, [id]);

  if (!student) return null;

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <Link href="/students" className="inline-flex items-center gap-2 text-xs font-bold text-zinc-400 hover:text-amber-400">
        <ArrowLeft className="w-4 h-4" /> Quay lại danh bạ
      </Link>
      <div className="glass-card rounded-3xl p-8 sm:p-12 border border-white/10 flex flex-col md:flex-row items-center gap-8">
        <div className="w-24 h-24 rounded-3xl bg-amber-500 flex items-center justify-center font-black text-black text-3xl">
          {student.fullName.charAt(0)}
        </div>
        <div className="space-y-2 text-center md:text-left">
          <h1 className="text-3xl font-black text-white">{student.fullName}</h1>
          <div className="text-xs text-amber-400 font-bold">{student.major} • Mã: {student.code}</div>
          <p className="text-xs text-zinc-400 max-w-xl">{student.bio}</p>
        </div>
      </div>
      <div className="space-y-6">
        <h2 className="text-2xl font-black text-white">Tác Phẩm Đã Sáng Tác</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {(student.paintings || []).map((p) => (
            <div key={p.id} className="glass-card rounded-2xl overflow-hidden border border-white/10">
              <img src={p.primaryImageUrl} alt={p.title} className="w-full aspect-[4/3] object-cover" />
              <div className="p-6 space-y-2">
                <h3 className="text-lg font-bold text-white">{p.title}</h3>
                <Link href={`/artworks/${p.id}`} className="inline-block px-3 py-1.5 rounded-lg bg-amber-500 text-black text-xs font-bold">
                  Xem Tranh
                </Link>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}