import { useState, useEffect } from "react";
import { useAppSelector } from "../../app/store/store";
import { useGetCoursesQuery } from "../course/courseApi";
import LoadingIndicator from "../../app/layout/LoadingIndicator";
import EmptyState from "../../app/layout/ui/EmptyState";
import { BookOpen } from "lucide-react";
import CourseFilters from "../course/CourseFilters";
import CourseList from "../course/CourseList";
import Pagination from "../course/Pagination";


const CourseCatalog = () => {
  const { user } = useAppSelector((s) => s.auth);
  const studentId = user?.id;

  // filters
  const [filters, setFilters] = useState({ gradeLevel: "", subject: "" });

  // search state with debounce
  const [searchInput, setSearchInput] = useState("");
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    const handler = setTimeout(() => {
      setSearchTerm(searchInput);
      // reset pagination when search changes
      setCursorStack([undefined]);
      setCursorIndex(0);
    }, 500);

    return () => clearTimeout(handler);
  }, [searchInput]);

  // cursor-based pagination
  const [cursorStack, setCursorStack] = useState<(number | undefined)[]>([
    undefined,
  ]);
  const [cursorIndex, setCursorIndex] = useState(0);
  const lastCourseId = cursorStack[cursorIndex];

  const { data, isLoading, isError } = useGetCoursesQuery({
    pageSize: 6,
    lastId: lastCourseId,
    gradeLevel: filters.gradeLevel,
    subject: filters.subject,
    searchTerm,
  });

  // filter handler
  const handleFilterChange = (key: string, value: string) => {
    setFilters((prev) => ({ ...prev, [key]: value }));
    setCursorStack([undefined]);
    setCursorIndex(0);
  };

  // pagination handlers
  const handleNext = () => {
    if (data?.hasNextPage && data?.lastId) {
      const newStack = [
        ...cursorStack.slice(0, cursorIndex + 1),
        data.lastId,
      ];
      setCursorStack(newStack);
      setCursorIndex(cursorIndex + 1);
    }
  };

  const handlePrevious = () => {
    if (cursorIndex > 0) {
      setCursorIndex(cursorIndex - 1);
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingIndicator
          variant="spinner"
          size="lg"
          colorClass="text-indigo-600"
          fullPage
        />
      </div>
    );
  }

  if (isError || !data || data.courses.length === 0) {
    return (
      <EmptyState
        icon={<BookOpen className="w-12 h-12 text-indigo-500" />}
        title="No courses available"
        description="Try adjusting your filters or search for something else."
      />
    );
  }

  return (
    <div className="p-6 space-y-6">
      {/* Filters + Search */}
      <CourseFilters
        filters={filters}
        searchInput={searchInput}
        onFilterChange={handleFilterChange}
        onSearchChange={setSearchInput}
      />

      {/* Course grid */}
      <CourseList courses={data.courses} studentId={studentId!} />

      {/* Pagination */}
      <Pagination
        onNext={handleNext}
        onPrevious={handlePrevious}
        canGoBack={cursorIndex > 0}
        canGoNext={!!data?.hasNextPage}
      />
    </div>
  );
};

export default CourseCatalog;









// import { useEffect, useState } from "react";
// import { BookOpen } from "lucide-react";
// import { useAppSelector } from "../../app/store/store";
// import { useGetCoursesQuery } from "../course/courseApi";
// import LoadingIndicator from "../../app/layout/LoadingIndicator";
// import EmptyState from "../../app/layout/ui/EmptyState";
// import CourseList from "../course/CourseList";

// export default function CourseCatalog() {
//   const { user } = useAppSelector((s) => s.auth);
//   const studentId = user?.id;

//   const [filters, setFilters] = useState({ gradeLevel: "", subject: "" });
//   const [searchInput, setSearchInput] = useState("");
//   const [searchTerm, setSearchTerm] = useState("");

//   // debounce search
//   useEffect(() => {
//     const handler = setTimeout(() => {
//       setSearchTerm(searchInput);
//       setCursorStack([undefined]);
//       setCursorIndex(0);
//     }, 500);
//     return () => clearTimeout(handler);
//   }, [searchInput]);

//   // cursor-based pagination
//   const [cursorStack, setCursorStack] = useState<(number | undefined)[]>([undefined]);
//   const [cursorIndex, setCursorIndex] = useState(0);
//   const lastCourseId = cursorStack[cursorIndex];

//   const { data, isLoading } = useGetCoursesQuery({
//     pageSize: 6,
//     lastId: lastCourseId,
//     searchTerm,
//     gradeLevel: filters.gradeLevel,
//     subject: filters.subject,
//   });

//   const handleNext = () => {
//     if (data?.hasNextPage && data?.lastId) {
//       const newStack = [...cursorStack.slice(0, cursorIndex + 1), data.lastId];
//       setCursorStack(newStack);
//       setCursorIndex(cursorIndex + 1);
//     }
//   };

//   const handlePrevious = () => {
//     if (cursorIndex > 0) setCursorIndex(cursorIndex - 1);
//   };

//   const handleFilterChange = (key: string, value: string) => {
//     setFilters((prev) => ({ ...prev, [key]: value }));
//     setCursorStack([undefined]);
//     setCursorIndex(0);
//   };

//   if (isLoading) {
//     return (
//       <div className="flex items-center justify-center h-64">
//         <LoadingIndicator variant="spinner" size="lg" colorClass="text-indigo-600" fullPage />
//       </div>
//     );
//   }

//   if (!data || data.courses?.length === 0) {
//     return (
//       <EmptyState
//         icon={<BookOpen className="w-12 h-12 text-indigo-500" />}
//         title="No courses available"
//         description="Try adjusting your filters or search for something else."
//       />
//     );
//   }

//   return (
//     <div className="p-6 space-y-6">
//       {/* Filters */}
//       <div className="sticky top-0 z-10 bg-white shadow-sm p-4 rounded-md mb-4">
//         <div className="flex flex-wrap gap-4 items-center">
//           <input
//             type="text"
//             placeholder="Search courses..."
//             value={searchInput}
//             onChange={(e) => setSearchInput(e.target.value)}
//             className="border rounded px-3 py-2 flex-1 min-w-[200px]"
//           />
//           <select
//             value={filters.gradeLevel}
//             onChange={(e) => handleFilterChange("gradeLevel", e.target.value)}
//             className="border rounded px-3 py-2"
//           >
//             <option value="">All Grades</option>
//             <option value="Grade1">Grade 1</option>
//             <option value="Grade6">Grade 6</option>
//             <option value="Grade8">Grade 8</option>
//             <option value="Grade10">Grade 10</option>
//           </select>
//           <select
//             value={filters.subject}
//             onChange={(e) => handleFilterChange("subject", e.target.value)}
//             className="border rounded px-3 py-2"
//           >
//             <option value="">All Subjects</option>
//             <option value="Math">Math</option>
//             <option value="Science">Science</option>
//             <option value="History">History</option>
//           </select>
//         </div>
//       </div>

//       {/* Course List */}
//       <CourseList
//         courses={data?.courses || []}
//         onEnroll={(courseId) => console.log("Enroll clicked for", courseId)}
//       />

//       {/* Pagination */}
//       <div className="flex justify-between mt-6">
//         <button
//           onClick={handlePrevious}
//           disabled={cursorIndex === 0}
//           className="px-4 py-2 rounded bg-gray-200 disabled:opacity-50"
//         >
//           ← Previous
//         </button>
//         <button
//           onClick={handleNext}
//           disabled={!data?.hasNextPage}
//           className="px-4 py-2 rounded bg-blue-500 text-white disabled:opacity-50"
//         >
//           Next →
//         </button>
//       </div>
//     </div>
//   );
// }






// import { useEffect, useState } from "react";
// import {useDebounce} from 'use-debounce';
// import { useAppSelector } from "../../app/store/store";
// import { useGetCoursesQuery } from "../course/courseApi";
// import LoadingIndicator from "../../app/layout/LoadingIndicator";
// import EmptyState from "../../app/layout/ui/EmptyState";
// import { BookOpen, Underline } from "lucide-react";
// import CourseCard from "../course/CourseCard";

// const grades = [
//   "Grade1", "Grade2", "Grade3", "Grade4", "Grade5", "Grade6",
//   "Grade7", "Grade8", "Grade9", "Grade10", "Grade11", "Grade12",
// ];

// const subjects = ["Math", "English", "Science", "History", "Art", "Music"];

//  const CourseCatalog = () => {
//   const { user } = useAppSelector((s) => s.auth);
//   const studentId = user?.id;

  
//   const [filters, setFilters] = useState({
//     gradeLevel: "",
//     subject: ""
//   });
  
//   // Search with debounce
//   const [searchInput, setSearchInput] = useState("");
//   const [searchTerm, setSearchTerm] = useState("");
  
  
//   useEffect(() => {
//     const handler = setTimeout(() => {
//       setSearchTerm(searchInput);
//       // reset pagination when search term changes 
//       setCursorStack([undefined]);
//       setCursorIndex(0);
//     }, 500);
    
//     return () => clearTimeout(handler);
    
//   }, [searchInput])
  
  
//   // pagination with cursor stack
//   const [cursorStack, setCursorStack] = useState<(number | undefined)[]>([undefined]);
//   const [cursorIndex, setCursorIndex] = useState(0);
//   const lastCourseId = cursorStack[cursorIndex];

//   const { data, isLoading } = useGetCoursesQuery({
//     pageSize: 6,
//     lastId: lastCourseId,
//     searchTerm,
//     gradeLevel: filters.gradeLevel,
//     subject: filters.subject
//   });

//   console.log("Courses: ", data?.courses);

//   // Pagination handlers 

//   const handleNext = () => {
//     if (data?.hasNextPage && data?.lastId) {
//       const newStack = [...cursorStack.slice(0, cursorIndex + 1), data.lastId];
//       setCursorStack(newStack);
//       setCursorIndex(cursorIndex + 1);
//     }
//   };

//   const handlePrevious = () => {
//     if (cursorIndex > 0){
//       setCursorIndex(cursorIndex - 1);
//     }
//   }


//   // Filter handler
//   const handleFilterChange = (key: string, value: string) => {
//     setFilters((prev) => ({...prev, [key]: value}));
//     setCursorStack([undefined]);
//     setCursorIndex(0);
//   }

//   if (isLoading) {
//     return (
//       <div className="flex items-center justify-center h-64">
//         <LoadingIndicator variant="spinner" size="lg" colorClass="text-indigo-600" />
//       </div>
//     );
//   }

//   if (!data || data.courses?.length === 0) {
//     return (
//       <EmptyState
//         icon={<BookOpen className="w-12 h-12 text-indigo-500" />}
//         title="No courses available"
//         description="Try adjusting your filters or search for something else."
//       />
//     );
//   }

//   return (
//      <div className="p-6 space-y-6">
//       {/* Sticky Search + Filters */}
//       <div className="sticky top-0 z-10 bg-white shadow-sm p-4 rounded-md mb-4">
//         <div className="flex flex-wrap gap-4 items-center">
//           {/* Search */}
//           <input
//             type="text"
//             placeholder="Search courses..."
//             value={searchInput}
//             onChange={(e) => setSearchInput(e.target.value)}
//             className="border rounded px-3 py-2 flex-1 min-w-[200px]"
//           />

//           {/* Grade filter */}
//           <select
//             value={filters.gradeLevel}
//             onChange={(e) => handleFilterChange("gradeLevel", e.target.value)}
//             className="border rounded px-3 py-2"
//           >
//             <option value="">All Grades</option>
//             <option value="Grade1">Grade 1</option>
//             <option value="Grade6">Grade 6</option>
//             <option value="Grade8">Grade 8</option>
//             <option value="Grade10">Grade 10</option>
//           </select>

//           {/* Subject filter */}
//           <select
//             value={filters.subject}
//             onChange={(e) => handleFilterChange("subject", e.target.value)}
//             className="border rounded px-3 py-2"
//           >
//             <option value="">All Subjects</option>
//             <option value="Math">Math</option>
//             <option value="Science">Science</option>
//             <option value="History">History</option>
//           </select>
//         </div>
//       </div>

//       {/* Course Grid */}
//       {isLoading ? (
//         <LoadingIndicator  variant="spinner" size="lg" colorClass="white-text" fullPage/>
//       ) : data?.courses.length ? (
//         <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
//           {data.courses.map((course) => (
//             <CourseCard key={course.id} {...course} studentId={studentId!}/>
//           ))}
//         </div>
//       ) : (
//         <EmptyState
//         icon={<BookOpen className="w-12 h-12 text-indigo-500" />}
//         title="No courses available"
//         description="Try adjusting your filters or search for something else."
//       />
//       )}

//       {/* Pagination */}
//       <div className="flex justify-between mt-6">
//         <button
//           onClick={handlePrevious}
//           disabled={cursorIndex === 0}
//           className="px-4 py-2 rounded bg-gray-200 disabled:opacity-50"
//         >
//           ← Previous
//         </button>

//         <button
//           onClick={handleNext}
//           disabled={!data?.hasNextPage}
//           className="px-4 py-2 rounded bg-blue-500 text-white disabled:opacity-50"
//         >
//           Next →
//         </button>
//       </div>
//     </div>
//   );
// };

// export default CourseCatalog;

// // <div className="p-6 space-y-8">
// //   {/* Search + Filters */}
// //   <div className="flex flex-col sm:flex-row gap-4">
// //     <input
// //       type="text"
// //       placeholder="Search courses..."
// //       value={searchInput}
// //       onChange={(e) => {
// //         setSearchInput(e.target.value );
// //      }}
// //       className="flex-1 border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400"
// //     />

// //     <select
// //       value={filters.gradeLevel}
// //       onChange={(e) => handleFilterChange("gradeLevel", e.target.value)}
// //       className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400"
// //     >
// //       <option value="">All Grades</option>
// //       {grades.map((grade) => (
// //         <option key={grade} value={grade}>
// //           {grade}
// //         </option>
// //       ))}
// //     </select>

// //     <select
// //       value={filters.subject}
// //       onChange={(e) => handleFilterChange("subject", e.target.value)}
// //       className="border rounded-xl p-2 shadow-sm focus:ring-2 focus:ring-indigo-400"
// //     >
// //       <option value="">All Subjects</option>
// //       {subjects.map((subject) => (
// //         <option key={subject} value={subject}>
// //           {subject}
// //         </option>
// //       ))}
// //     </select>
// //   </div>

// //   {/* Course Grid */}
// //   <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
// //     {data.courses?.map((course) => (
// //       <CourseCard key={course.id} {...course} studentId={studentId!} />
// //     ))}
// //   </div>

// //   {/* Pagination */}
// //   <div className="flex justify-between mt-6">
// //     <button
// //       onClick={handlePrevious}
// //       className="px-4 py-2 bg-gray-200 rounded-lg shadow hover:bg-gray-300 transition disabled:opacity-50"
// //       disabled={cursorIndex === 0}
// //     >
// //         ← Previous
// //     </button>
// //     <button
// //       onClick={handleNext}
// //       className="px-4 py-2 bg-indigo-600 text-white rounded-lg shadow hover:bg-indigo-700 transition disabled:opacity-50"
// //       disabled={!data.hasNextPage}
// //     >
// //       Next →
// //     </button>
// //   </div>
// // </div>