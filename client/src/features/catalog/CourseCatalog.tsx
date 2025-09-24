import LoadingIndicator from "../../app/layout/LoadingIndicator";
import { useGetCoursesQuery } from "./courseApi"

const CourseCatalog = () => {
  const {data: courses, isLoading, error} = useGetCoursesQuery();

  if (isLoading) return <LoadingIndicator variant="dots" size="lg" colorClass="text-white" />;
  if (error) return <p className="text-center text-red-500">Failed to load courses</p>;

  
  return (
      <div className="p-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      {courses?.map((course) => (
        <div
          key={course.id}
          className="rounded-2xl bg-white shadow-md p-4 hover:shadow-lg transition"
        >
          <h2 className="text-lg font-bold text-indigo-600">{course.title}</h2>
          <p className="text-gray-600">{course.description}</p>
          <p className="mt-2 text-sm font-medium">
            <span className="text-gray-500">Subject:</span> {course.subject}
          </p>
          <p className="text-sm font-medium">
            <span className="text-gray-500">Grade:</span> {course.gradeLevel}
          </p>
          <p className="text-sm font-medium">
            <span className="text-gray-500">Teacher:</span> {course.teacherName}
          </p>

          {course.sessions.length > 0 && (
            <div className="mt-2 text-xs text-gray-500">
              <p>📺 Next session: {new Date(course.sessions[0].startTime).toLocaleString()}</p>
            </div>
          )}
        </div>
      ))}
    </div>
  )
}

export default CourseCatalog
