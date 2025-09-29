import { useState } from "react";
import { useAppSelector } from "../../app/store/store";
import { useGetCoursesQuery } from "../course/courseApi";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import EmptyState from "../../app/layout/ui/EmptyState";
import { BookOpen } from "lucide-react";
import CourseCard from "../course/CourseCard";

const grades = [
  "Grade1", "Grade2", "Grade3", "Grade4", "Grade5", "Grade6",
  "Grade7", "Grade8", "Grade9", "Grade10", "Grade11", "Grade12",
];

const subjects = ["Math", "English", "Science", "History", "Art", "Music"];

 const CourseCatalog = () => {
  const { user } = useAppSelector((s) => s.auth);
  const studentId = user?.id;

  const [filters, setFilters] = useState({
    searchTerm: "",
    gradeLevel: "",
    subject: ""
  });

  const [lastId, setLastId] = useState<number | undefined>();

  const { data, isLoading } = useGetCoursesQuery({
    pageSize: 6,
    lastId,
    searchTerm: filters.searchTerm,
    gradeLevel: filters.gradeLevel,
    subject: filters.subject
  });

  console.log("Courses: ", data?.courses);

  const handleNext = () => {
    if (data?.hasNextPage && data?.lastId) {
      setLastId(data.lastId);
    }
  };

  const handleReset = () => setLastId(undefined);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingIndicator variant="spinner" size="lg" colorClass="text-indigo-600" />
      </div>
    );
  }

  if (!data || data.courses?.length === 0) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12 text-indigo-500" />}
        title="No courses available"
        description="Try adjusting your filters or search for something else."
      />
    );
  }

  return (
    <div className="p-6 space-y-8">
      {/* Search + Filters */}
      <div className="flex flex-col sm:flex-row gap-4">
        <input
          type="text"
          placeholder="Search courses..."
          value={filters.searchTerm}
          onChange={(e) => {
            setFilters({ ...filters, searchTerm: e.target.value });
            setLastId(undefined);
          }}
          className="flex-1 border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400"
        />

        <select
          value={filters.gradeLevel}
          onChange={(e) => {
            setFilters({ ...filters, gradeLevel: e.target.value });
            setLastId(undefined);
          }}
          className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400"
        >
          <option value="">All Grades</option>
          {grades.map((grade) => (
            <option key={grade} value={grade}>
              {grade}
            </option>
          ))}
        </select>

        <select
          value={filters.subject}
          onChange={(e) => {
            setFilters({ ...filters, subject: e.target.value });
            setLastId(undefined);
          }}
          className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400"
        >
          <option value="">All Subjects</option>
          {subjects.map((subject) => (
            <option key={subject} value={subject}>
              {subject}
            </option>
          ))}
        </select>
      </div>

      {/* Course Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {data.courses?.map((course) => (
          <CourseCard key={course.id} {...course} studentId={studentId!} />
        ))}
      </div>

      {/* Pagination */}
      <div className="flex justify-between mt-6">
        <button
          onClick={handleReset}
          className="px-4 py-2 bg-gray-200 rounded-lg shadow hover:bg-gray-300 transition disabled:opacity-50"
          disabled={!lastId}
        >
          Reset
        </button>
        <button
          onClick={handleNext}
          className="px-4 py-2 bg-indigo-600 text-white rounded-lg shadow hover:bg-indigo-700 transition disabled:opacity-50"
          disabled={!data.hasNextPage}
        >
          Next →
        </button>
      </div>
    </div>
  );
};

export default CourseCatalog;
