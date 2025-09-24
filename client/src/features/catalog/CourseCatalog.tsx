// features/catalog/CourseCatalog.tsx
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import EmptyState from "../../app/layout/ui/EmptyState";
import { useAppSelector } from "../../app/store/store";
import { useGetCoursesQuery } from "../course/courseApi";
import CourseCard from "../course/CourseCard";
import { BookOpen } from "lucide-react";

const CourseCatalog = () => {
  const { user } = useAppSelector(state => state.auth);
  const studentId = user?.id;

  const { data: courses, isLoading, isError } = useGetCoursesQuery();

  if (isLoading) return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" />;

  if (isError || !courses || courses.length === 0) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12" />}
        title="No courses available"
        description="Please check back later or contact admin."
      />
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 p-4">
      {courses.map(course => (
        <CourseCard
          key={course.id}
          id={course.id}
          title={course.title}
          description={course.description}
          subject={course.subject}
          gradeLevel={course.gradeLevel}
          teacherName={course.teacherName}
          studentId={studentId!} // studentId is guaranteed because user must be logged in
        />
      ))}
    </div>
  );
};

export default CourseCatalog;



// import LoadingIndicator from "../../app/layout/LoadingIndicator";
// import { useGetCoursesQuery } from "../course/courseApi"

// const CourseCatalog = () => {
//   const {data: courses, isLoading, error} = useGetCoursesQuery();

//   if (isLoading) return <LoadingIndicator variant="dots" size="lg" colorClass="text-white" />;
//   if (error) return <p className="text-center text-red-500">Failed to load courses</p>;

  
//   return (
//       <div className="p-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
//       {courses?.map((course) => (
//         <div
//           key={course.id}
//           className="rounded-2xl bg-white shadow-md p-4 hover:shadow-lg transition"
//         >
//           <h2 className="text-lg font-bold text-indigo-600">{course.title}</h2>
//           <p className="text-gray-600">{course.description}</p>
//           <p className="mt-2 text-sm font-medium">
//             <span className="text-gray-500">Subject:</span> {course.subject}
//           </p>
//           <p className="text-sm font-medium">
//             <span className="text-gray-500">Grade:</span> {course.gradeLevel}
//           </p>
//           <p className="text-sm font-medium">
//             <span className="text-gray-500">Teacher:</span> {course.teacherName}
//           </p>

//           {course.sessions.length > 0 && (
//             <div className="mt-2 text-xs text-gray-500">
//               <p>📺 Next session: {new Date(course.sessions[0].startTime).toLocaleString()}</p>
//             </div>
//           )}
//         </div>
//       ))}
//     </div>
//   )
// }

// export default CourseCatalog
