import { useState } from "react";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import EmptyState from "../../app/layout/ui/EmptyState";
import { useAppSelector } from "../../app/store/store";
import { useGetCoursesQuery } from "../course/courseApi";
import CourseCard from "../course/CourseCard";
import { BookOpen } from "lucide-react";

const grades = [
  "Grade1", "Grade2", "Grade3", "Grade4", "Grade5", "Grade6",
  "Grade7", "Grade8", "Grade9", "Grade10", "Grade11", "Grade12",
];

const subjects = ["Math", "English", "Science", "History", "Art", "Music"];

const CourseCatalog = () => {
  const { user } = useAppSelector((state) => state.auth);
  const studentId = user?.id;

  // State for filters and pagination
  const [selectedGrade, setSelectedGrade] = useState<string | undefined>();
  const [selectedSubject, setSelectedSubject] = useState<string | undefined>();
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [pageNumber, setPageNumber] = useState(1);

  const { data, isLoading } = useGetCoursesQuery({
    pageNumber,
    pageSize: 6,
    searchTerm: searchTerm || undefined,
    gradeLevel: selectedGrade,
    subject: selectedSubject,
  });

  console.log(data?.metaData);

  if (isLoading) {
    return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" fullPage  />;
  }

  if (!data || data.data.length === 0) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12" />}
        title="No courses available"
        description="Try adjusting your filters or check back later."
      />
    );
  }

  return (
    <div className="p-4 space-y-6">
      {/* Search + Filters */}
      <div className="flex flex-col sm:flex-row gap-4 items-center">
        <input
          type="text"
          placeholder="Search courses..."
          value={searchTerm}
          onChange={(e) => {
            setSearchTerm(e.target.value);
            setPageNumber(1); // reset to page 1
          }}
          className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400 w-full sm:w-1/3"
        />

        <select
          value={selectedGrade || ""}
          onChange={(e) => {
            setSelectedGrade(e.target.value || undefined);
            setSelectedSubject(undefined); // reset subject when grade changes
            setPageNumber(1);
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
          value={selectedSubject || ""}
          onChange={(e) => {
            setSelectedSubject(e.target.value || undefined);
            setPageNumber(1);
          }}
          disabled={!selectedGrade} // subjects only after grade chosen
          className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400 disabled:bg-gray-100 disabled:text-gray-400"
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
        {data.data.map((course) => (
          <CourseCard
            key={course.id}
            id={course.id}
            title={course.title}
            description={course.description}
            subject={course.subject}
            gradeLevel={course.gradeLevel}
            teacherName={course.teacherName}
            studentId={studentId!}
          />
        ))}
      </div>

      {/* Pagination */}
      <div className="flex justify-center items-center gap-4 mt-6">
        <button
          disabled={pageNumber === 1}
          onClick={() => setPageNumber((p) => p - 1)}
          className="px-3 py-1 rounded-lg bg-indigo-600 text-white disabled:bg-gray-300"
        >
          Prev
        </button>

        <span className="text-gray-700">
          Page {data.metaData.currentPage} of {data.metaData.totalPages}
        </span>

        <button
          disabled={pageNumber === data.metaData.totalPages}
          onClick={() => setPageNumber((p) => p + 1)}
          className="px-3 py-1 rounded-lg bg-indigo-600 text-white disabled:bg-gray-300"
        >
          Next
        </button>
      </div>
    </div>
  );
};

export default CourseCatalog;
