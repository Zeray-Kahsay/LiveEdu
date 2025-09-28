import { apiSlice } from "../../app/api/apiSlice";
import type { CourseDto } from "../../app/types/course/courseDto";



interface MetaDta {
    currentPage: number;
    totalPages: number;
    totalCount: number;
    pageSize: number;
}

interface CourseResponse {
    data: CourseDto[];
    metaData: MetaDta;
}


export const courseApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getCourses: builder.query<CourseResponse, {pageNumber: number; pageSize: number; searchTerm?: string; gradeLevel?:string; subject?:string }>({
            query: ({pageNumber, pageSize, searchTerm, gradeLevel, subject}) => {
              const params = new URLSearchParams({
                pageNumber: pageNumber.toString(),
                pageSize: pageSize.toString(),
              });

              if (searchTerm) params.append("searchTerm", searchTerm);
              if (gradeLevel)params.append("gradeLevel", gradeLevel);
              if (subject) params.append("subject", subject);

              return `/courses?${params.toString()}`;
            },

            transformResponse: (response: CourseDto[], meta): CourseResponse => {
                let metaData = {
                    currentPage: 1,
                    totalPages: 2,
                    totalCount: 0,
                    pageSize: 0
                };

              if (meta?.response?.headers){
                const paginationHeader = meta.response.headers.get("X-Pagination");
                  if (paginationHeader){
                    metaData = JSON.parse(paginationHeader);
                 }
               }
               return {data: response, metaData};
            
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