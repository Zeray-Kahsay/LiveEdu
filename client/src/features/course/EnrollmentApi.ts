import { apiSlice } from "../../app/api/apiSlice";

type EnrollRequest = {
    courseId: number;
    studentId: number;
};

type EnrollResponse = {
    enrollmentId: number;
    courseId: number;
    studentId: number;
    status: string;
    courseTitle: string;
}


export const enrollmentApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    enrollCourse: builder.mutation<EnrollResponse, EnrollRequest>({
      query: ({ courseId, studentId }) => ({
        url: "/enrollments",
        method: "POST",
        body: { courseId, studentId },
      }),
    }),
    getStudentEnrollments: builder.query<EnrollResponse[], number>({
      query: (studentId) => `/enrollments/student/${studentId}`,
    }),
  }),
});

export const {
     useEnrollCourseMutation,
      useGetStudentEnrollmentsQuery ,
    } = enrollmentApi;
