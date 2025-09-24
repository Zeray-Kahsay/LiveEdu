import type { SessionDto } from "./SessionDto";

export interface CourseDto {
  id: number;
  title: string;
  description: string;
  subject: string;
  gradeLevel: string;
  teacherName: string;
  sessions: SessionDto[];
}