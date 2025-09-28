import { useOutletContext, useParams } from "react-router-dom";
import type { dashboardContext } from "../../app/types/dashboard/DashboardContext";
import { useGetEnrollmentByCourseAndStudentQuery } from "./EnrollmentApi";
import { useGetCourseByIdQuery } from "./courseApi";
import LoadingIndicator from "../../app/layout/LoadingIndicator";

const CourseDetails = () => {
  const {id} = useParams();
  const courseId = Number(id);
  const {studentId} = useOutletContext<dashboardContext>();
 
  // Fetch course info
  const {data: course, isLoading: courseLoading } = useGetCourseByIdQuery(courseId);

  // check if the student is enrolled in this course 
  const {data: enrollment, isLoading: enrollmentLoading} = useGetEnrollmentByCourseAndStudentQuery({courseId, studentId});

  if (courseLoading || enrollmentLoading){
    return <LoadingIndicator variant = "spinner" size="lg" colorClass="white-text"fullPage />
  }


  // If not enrolled
  if (!enrollment) {
    return (
      <div className="p-6 text-center">
        <h2 className="text-2xl font-semibold mb-4 text-red-500">Access Denied</h2>
        <p className="text-gray-600 mb-4">
          You are not enrolled in <span className="font-semibold">{course?.title}</span>.
        </p>
        <p className="text-gray-500">Please enroll first from the course catalog.</p>
      </div>
    );
  }

  
  return ( 
     <div className="p-6">
      <h1 className="text-3xl font-semibold">{course?.title}</h1>
      <p className="text-gray-600 mb-4">
        {course?.subject} • Grade  {course?.gradeLevel}
      </p>
      <p> {course?.description}</p>
      <p className="text-sm text-gray-500 mb-6">👨‍🏫 {course?.teacherName}</p>
      <h2 className="text-xl font-semibold mb-2">📅 Sessions</h2>
        <ul className="list-disc pl-6 space-y-1">
          {enrollment?.sessions?.map((s) => (
           <li key={s.sessionId}>
             <span className="font-medium">{s.title}</span> –{" "}
             {new Date(s.startTime).toLocaleString()} → {new Date(s.endTime).toLocaleString()}{" "}
            <a href={s.streamUrl} target="_blank" rel="noopener noreferrer" className="text-blue-600 underline ml-2">
                Join
           </a>
         </li>
        ))}
</ul>

     
    </div>
  )
}

export default CourseDetails
