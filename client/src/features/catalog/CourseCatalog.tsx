// features/catalog/CourseCatalog.tsx
import { useState } from "react";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import EmptyState from "../../app/layout/ui/EmptyState";
import { useAppSelector } from "../../app/store/store";
import { useGetCoursesByFilterQuery, useGetCoursesQuery } from "../course/courseApi";
import CourseCard from "../course/CourseCard";
import { BookOpen } from "lucide-react";

const grades = [
  "Grade1","Grade2","Grade3","Grade4","Grade5","Grade6",
  "Grade7","Grade8","Grade9","Grade10","Grade11","Grade12",
];

const subjects = ["Math", "English", "Science", "History", "Art", "Music"];

const CourseCatalog = () => {
  const [selectedGrade, setSelectedGrade] = useState<string | undefined>();
  const [selectedSubject, setSelectedSubject] = useState<string | undefined>();

  const { user } = useAppSelector((state) => state.auth);
  const studentId = user?.id;

  // Queries
  const {
    data: filteredCourses,
    isLoading: filterIsLoading,
    isError: filterIsError,
  } = useGetCoursesByFilterQuery(
    { grade: selectedGrade, subject: selectedSubject },
    { skip: !selectedGrade && !selectedSubject }
  );

  const {
    data: allCourses,
    isLoading: allCoursesIsLoading,
    isError: allCoursesIsError,
  } = useGetCoursesQuery();

  // Determine which dataset to show
  const isFiltering = !!(selectedGrade || selectedSubject);
  const courses = isFiltering ? filteredCourses : allCourses;

  // Loading state
  if (filterIsLoading || allCoursesIsLoading) {
    return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" />;
  }

  // Empty / error states
  if (isFiltering && (filterIsError || !filteredCourses?.length)) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12" />}
        title="No courses match your filter"
        description="Try adjusting grade or subject."
      />
    );
  }

  if (!isFiltering && (allCoursesIsError || !allCourses?.length)) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12" />}
        title="No courses available"
        description="Please check back later or contact admin."
      />
    );
  }

  return (
    <div className="p-6 space-y-6">
      {/* Filters */}
      <div className="flex flex-wrap gap-4">
        <div className="flex flex-col">
          <label className="text-sm font-medium text-gray-600 mb-1">Grade</label>
          <select
            value={selectedGrade || ""}
            onChange={(e) => setSelectedGrade(e.target.value || undefined)}
            className="rounded-xl border border-gray-300 bg-white px-4 py-2 shadow-sm focus:border-indigo-500 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
          >
            <option value="">All Grades</option>
            {grades.map((grade) => (
              <option key={grade} value={grade}>
                {grade}
              </option>
            ))}
          </select>
        </div>

        <div className="flex flex-col">
          <label className="text-sm font-medium text-gray-600 mb-1">Subject</label>
          <select
            value={selectedSubject || ""}
            onChange={(e) => setSelectedSubject(e.target.value || undefined)}
            className="rounded-xl border border-gray-300 bg-white px-4 py-2 shadow-sm focus:border-indigo-500 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
          >
            <option value="">All Subjects</option>
            {subjects.map((subject) => (
              <option key={subject} value={subject}>
                {subject}
              </option>
            ))}
          </select>
        </div>
      </div>

      {/* Courses Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {courses?.map((course) => (
          <CourseCard
            key={course.id}
            id={course.id}
            title={course.title}
            description={course.description}
            subject={course.subject}
            gradeLevel={course.gradeLevel}
            teacherName={course.teacherName}
            studentId={studentId!} // user must be logged in
          />
        ))}
      </div>
    </div>
  );
};

export default CourseCatalog;





// // features/catalog/CourseCatalog.tsx
// import { useState } from "react";
// import LoadingIndicator from "../../app/layout/LoadingIndicator";
// import EmptyState from "../../app/layout/ui/EmptyState";
// import { useAppSelector } from "../../app/store/store";
// import { useGetCoursesByFilterQuery, useGetCoursesQuery } from "../course/courseApi";
// import CourseCard from "../course/CourseCard";
// import { BookOpen } from "lucide-react";

// const CourseCatalog = () => {
//   const [selectedGrade, setSelectedGrade] = useState<string | undefined>();
//   const [selectedSubject, setSelectedSubject] = useState<string | undefined>();
//   const { user } = useAppSelector(state => state.auth);
//   const studentId = user?.id;

//   const {} = useGetCoursesByFilterQuery({
//     grade: selectedGrade,
//     subject: selectedSubject
//   });

//   const { data: courses, isLoading, isError } = useGetCoursesQuery();

//   if (isLoading) return <LoadingIndicator variant="spinner" size="lg" colorClass="white-text" />;

//   if (isError || !courses || courses.length === 0) {
//     return (
//       <EmptyState
//         icon={<BookOpen className="w-12 h-12" />}
//         title="No courses available"
//         description="Please check back later or contact admin."
//       />
//     );
//   }

//   return (
//     <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 p-4">
//       {courses.map(course => (
//         <CourseCard
//           key={course.id}
//           id={course.id}
//           title={course.title}
//           description={course.description}
//           subject={course.subject}
//           gradeLevel={course.gradeLevel}
//           teacherName={course.teacherName}
//           studentId={studentId!} // studentId is guaranteed because user must be logged in
//         />
//       ))}
//     </div>
//   );
// };

// export default CourseCatalog;



// // import LoadingIndicator from "../../app/layout/LoadingIndicator";
// // import { useGetCoursesQuery } from "../course/courseApi"

// // const CourseCatalog = () => {
// //   const {data: courses, isLoading, error} = useGetCoursesQuery();

// //   if (isLoading) return <LoadingIndicator variant="dots" size="lg" colorClass="text-white" />;
// //   if (error) return <p className="text-center text-red-500">Failed to load courses</p>;

  
// //   return (
// //       <div className="p-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
// //       {courses?.map((course) => (
// //         <div
// //           key={course.id}
// //           className="rounded-2xl bg-white shadow-md p-4 hover:shadow-lg transition"
// //         >
// //           <h2 className="text-lg font-bold text-indigo-600">{course.title}</h2>
// //           <p className="text-gray-600">{course.description}</p>
// //           <p className="mt-2 text-sm font-medium">
// //             <span className="text-gray-500">Subject:</span> {course.subject}
// //           </p>
// //           <p className="text-sm font-medium">
// //             <span className="text-gray-500">Grade:</span> {course.gradeLevel}
// //           </p>
// //           <p className="text-sm font-medium">
// //             <span className="text-gray-500">Teacher:</span> {course.teacherName}
// //           </p>

// //           {course.sessions.length > 0 && (
// //             <div className="mt-2 text-xs text-gray-500">
// //               <p>📺 Next session: {new Date(course.sessions[0].startTime).toLocaleString()}</p>
// //             </div>
// //           )}
// //         </div>
// //       ))}
// //     </div>
// //   )
// // }

// // export default CourseCatalog
