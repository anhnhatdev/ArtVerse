export type Role = 'Admin' | 'Curator' | 'Teacher' | 'Student' | 'Guest';

export interface User {
  id: string;
  userName: string;
  email: string;
  fullName: string;
  role: Role;
  avatarUrl?: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  user: User;
}

export interface PaintingDto {
  id: string;
  title: string;
  description?: string;
  medium?: string;
  dimensions?: string;
  yearCreated: number;
  price?: number;
  isForSale: boolean;
  status: 'Pending' | 'Approved' | 'Rejected' | 'Draft';
  curatorNotes?: string;
  createdAt: string;
  studentId: string;
  studentName: string;
  studentCode: string;
  primaryImageUrl?: string;
}

export interface CompetitionDto {
  id: string;
  title: string;
  description?: string;
  bannerUrl?: string;
  startDate: string;
  endDate: string;
  status: 'Upcoming' | 'Active' | 'UnderJudging' | 'Completed';
  totalEntries: number;
  criterias?: { id: string; name: string; maxScore: number; weight: number }[];
}

export interface CompetitionEntryDto {
  id: string;
  competitionId: string;
  competitionTitle: string;
  paintingId: string;
  paintingTitle: string;
  paintingImageUrl?: string;
  studentId: string;
  studentName: string;
  studentCode: string;
  submissionDate: string;
  status: 'Submitted' | 'Scored' | 'Disqualified';
  averageScore?: number;
  totalJudgesScored: number;
}

export interface LeaderboardEntryDto {
  rank: number;
  entryId: string;
  paintingId: string;
  paintingTitle: string;
  paintingImageUrl?: string;
  studentName: string;
  studentCode: string;
  averageScore: number;
  awardTitle?: string;
}

export interface ExhibitionDto {
  id: string;
  title: string;
  theme?: string;
  description?: string;
  bannerUrl?: string;
  startDate: string;
  endDate: string;
  status: 'Upcoming' | 'Ongoing' | 'Closed';
  curatorName?: string;
  totalArtworks: number;
  artworks?: {
    id: string;
    paintingId: string;
    title: string;
    studentName: string;
    imageUrl?: string;
    medium?: string;
    likeCount: number;
  }[];
}

export interface StudentDto {
  id: string;
  fullName: string;
  code: string;
  email: string;
  phone?: string;
  avatarUrl?: string;
  major?: string;
  bio?: string;
  enrollmentYear: number;
  totalArtworks: number;
  paintings?: PaintingDto[];
}

export interface ClassRoomDto {
  id: string;
  name: string;
  code: string;
  academicYear: string;
  semester: string;
  subjectName?: string;
  teacherName?: string;
  totalStudents: number;
}

export interface DashboardStatsDto {
  totalStudents: number;
  totalPaintings: number;
  approvedPaintings: number;
  pendingPaintings: number;
  totalCompetitions: number;
  activeCompetitions: number;
  totalExhibitions: number;
}