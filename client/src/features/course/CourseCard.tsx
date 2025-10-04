import { useNavigate } from "react-router-dom";
import type { CourseDto } from "../../app/types/course/courseDto";

interface CourseCardProps {
  course: CourseDto;
  studentId?: number;
}

const CourseCard = ({ course }: CourseCardProps) => {
  const navigate = useNavigate();

  return (
    <div
      onClick={() => navigate(`/courses/${course.id}`)}
      className="group relative bg-white rounded-2xl shadow-md overflow-hidden hover:shadow-lg transition cursor-pointer"
    >
      {/* Thumbnail placeholder */}
      <div className="h-40 bg-gradient-to-r from-indigo-500 to-purple-500 flex items-center justify-center text-white text-lg font-bold font-serif">
        {course.title.toLocaleUpperCase()}
      </div>

      <div className="p-4 space-y-2">
        <h3 className="text-lg font-semibold text-gray-800 group-hover:text-indigo-600 transition">
          {course.title}
        </h3>
        <p className="text-sm text-gray-600 line-clamp-2">{course.description}</p>

        <div className="flex justify-between items-center text-sm mt-3">
          <span className="px-2 py-1 rounded-full bg-gray-100 text-gray-700">
            {course.subject}
          </span>
          <span className="text-indigo-600 font-medium">{course.gradeLevel}</span>
        </div>
      </div>
    </div>
  );
};

export default CourseCard;
