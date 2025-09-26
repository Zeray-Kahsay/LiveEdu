import { apiSlice } from "../../app/api/apiSlice";

type EnrollRequest = {
    courseId: number;
    studentId: number;
};

type EnrollResponse = {
  enrollmentId: number;
  courseId: number;
  courseTitle: string;
  subject: string;
  gradeLevel: string;
  teacherName: string;
  enrolledAt: string;
  status: string;
}

export type Session = {
  sessionId: number;
  title: string;
  startTime: string;   // ISO date from API
  endTime: string;
  streamUrl: string;
  isLive: boolean;
};

export type Enrollment = {
  enrollmentId: number;
  courseId: number;
  courseTitle: string;
  description: string;
  subject: string;
  gradeLevel: string;
  teacherName: string;
  sessions: Session[];   
};



export const enrollmentApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    enrollCourse: builder.mutation<EnrollResponse, EnrollRequest>({
      query: ({ courseId, studentId }) => ({
        url: "/enrollments/enrollCourse",
        method: "POST",
        body: { courseId, studentId },
      }),
    }),
    getStudentEnrollments: builder.query<EnrollResponse[], number>({
      query: (studentId) => `/enrollments/student/${studentId}`,
    }),
    getEnrollmentByCourseAndStudent: builder.query<Enrollment, {courseId: number, studentId: number}>({
      query: ({courseId, studentId}) => `/enrollments/student/${studentId}/course/${courseId}`,
      providesTags: ["Enrollments"],
    })
  }),// endpoints 
});

export const {
     useEnrollCourseMutation,
      useGetStudentEnrollmentsQuery ,
      useGetEnrollmentByCourseAndStudentQuery,
    } = enrollmentApi;
