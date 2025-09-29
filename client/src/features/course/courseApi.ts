import { apiSlice } from "../../app/api/apiSlice";
import type { CourseDto } from "../../app/types/course/courseDto";


export interface CourseResponse {
    courses: CourseDto[];
    hasNextPage: boolean;
    lastId: number | null;
}

export interface CourseParams{
    courses: CourseDto[];
    hasNextPage: boolean;
    lastId: number;

}


export const courseApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getCourses: builder.query<CourseResponse, { lastId?: number; searchTerm?: string; gradeLevel?: string; subject?: string; pageSize?: number }>({
      query: ({ lastId, searchTerm, gradeLevel, subject }) => {
        const params = new URLSearchParams();

        if (lastId) params.append("lastId", lastId.toString());
        if (searchTerm) params.append("searchTerm", searchTerm);
        if (gradeLevel) params.append("gradeLevel", gradeLevel);
        if (subject) params.append("subject", subject);

        return {
          url: "/courses/getCourses",  
          method: "GET",
          params,
        };
      },
      transformResponse: (response: any): CourseResponse => {
        return {
          courses: response.courses,
          hasNextPage: response.hasNextPage,
          lastId: response.lastId,
        };
      },
      providesTags: ["Courses"],
    }),
     
        getCourseById: builder.query<CourseDto, number>({
            query: (id) => `/courses/${id}`,
            providesTags: (_result, _error, id) => [{type: "Courses", id}], 
        }),
        getCoursesByFilter: builder.query<CourseDto[], {grade?: string; subject?: string}>({
            query: ({grade, subject}) => {
                const params = new URLSearchParams();
                if (grade) params.append("GradeLevel", grade);
                if (subject) params.append("Subject", subject);
                return `/courses/filter?${params.toString()}`
            },
            providesTags: ["Courses"]
        })
      
    }),// endpoints 
});

export const {
    useGetCoursesQuery, 
    useGetCourseByIdQuery,
    useGetCoursesByFilterQuery,
    //useEnrollCourseMutation
} = courseApi;