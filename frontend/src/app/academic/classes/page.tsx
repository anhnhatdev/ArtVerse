'use client';
import React, { useState, useEffect } from 'react';
import { GraduationCap } from 'lucide-react';
import { apiClient } from '@/lib/api-client';
import { ClassRoomDto } from '@/types/api';

export default function AcademicClassesPage() {
  const [classes, setClasses] = useState<ClassRoomDto[]>([]);

  useEffect(() => {
    apiClient.get('/academic/classes').then(res => setClasses(res.data || [])).catch(() => {
      setClasses([
        { id: 'c-1', name: 'Lớp Hội Họa K21-A', code: 'HH21A', academicYear: '2023-2024', semester: 'Kỳ II', subjectName: 'Sơn Dầu Nâng Cao', teacherName: 'ThS. Nguyễn Văn Toàn', totalStudents: 28 },
        { id: 'c-2', name: 'Lớp Lụa & Dân Gian K22', code: 'LG22', academicYear: '2023-2024', semester: 'Kỳ II', subjectName: 'Tranh Lụa Cổ Điển', teacherName: 'ThS. Trần Thu Nga', totalStudents: 24 }
      ]);
    });
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-10">
      <div className="space-y-3">
        <div className="text-xs font-bold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
          <GraduationCap className="w-4 h-4" /> Quản Lý Đào Tạo
        </div>
        <h1 className="text-3xl sm:text-5xl font-black text-white tracking-tight">Danh Sách Lớp Học & Niên Khóa</h1>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        {classes.map(c => (
          <div key={c.id} className="glass-card rounded-2xl p-6 border border-white/10 space-y-3">
            <h3 className="text-xl font-bold text-white">{c.name} ({c.code})</h3>
            <div className="text-xs text-amber-400 font-semibold">{c.subjectName}</div>
            <div className="text-xs text-zinc-400">Giảng viên: <strong className="text-zinc-200">{c.teacherName}</strong> • {c.totalStudents} sinh viên</div>
          </div>
        ))}
      </div>
    </div>
  );
}