import type { CourseDto } from "../../app/types/course/courseDto";
import CourseCard from "./CourseCard";

interface CourseListProps {
  courses: CourseDto[];
  studentId?: number;
}

const CourseList = ({ courses, studentId }: CourseListProps) => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      {courses.map((course) => (
        <CourseCard key={course.id} course={course} studentId={studentId} />
      ))}
    </div>
  );
};

export default CourseList;
