import { fetchBaseQuery, type BaseQueryApi, type FetchArgs } from "@reduxjs/toolkit/query";
import { startLoading, stopLoading } from "../layout/uiSlice";
import { toast } from "react-toastify";
import { setCredentials, logout } from "../../features/auth/authSlice";
import type { RootState } from "../store/store";
import type { AuthResponse } from "../types/account/authResponse";

const customBaseQuery = fetchBaseQuery({
    baseUrl: 'https://localhost:5001/api',
     prepareHeaders: (headers) => {
    const authData = localStorage.getItem('auth');
    if (authData){
        const parsed = JSON.parse(authData);
        const token = parsed.accessToken;
        if (token) headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  },
});


// Re-fresh token
export const baseQueryWithReauth: typeof customBaseQuery = async (args, api, extraOptions) => {
  let result = await customBaseQuery(args, api, extraOptions);

  if (result.error && result.error.status === 401) {
    const refreshToken = (api.getState() as RootState).auth.refreshToken;

    if (refreshToken) {
      const refreshResult = await customBaseQuery(
        {
          url: "/auth/refresh-token",
          method: "POST",
          body: { refreshToken },
        },
        api,
        extraOptions
      );

      if (refreshResult.data) {
        //  tell TS that it’s AuthResponse
        const authData = refreshResult.data as AuthResponse;
        api.dispatch(setCredentials(authData));

        // retry original request with new token
        result = await customBaseQuery(args, api, extraOptions);
      } else {
        api.dispatch(logout());
        window.location.href = "/login";
      }
    } else {
      api.dispatch(logout());
      window.location.href = "/login";
    }
  }

  return result;
};


// export const baseQueryWithReauth: typeof customBaseQuery = async(args, api, extraOptions) => {
//     let result = await customBaseQuery(args, api, extraOptions);

//     if (result.error && result.error.status === 401){
//         // try refresh
//         const refreshResult = await customBaseQuery(
//             {url: "/auth/refresh-token", method: "POST"},
//             api,
//             extraOptions
//         );

//         if(refreshResult.data){
//             // SAVE new token
//             api.dispatch(setCredentials(refreshResult.data));
//             // retry original query
//             result = await customBaseQuery(args, api, extraOptions);
//         } else {
//             // refresh also failed -> logout
//             api.dispatch(logout());
//             window.location.href = "/login";
//         }
//     }

//     return result;

// }

const sleep = () => new Promise((resolve) => setTimeout(resolve, 1000));

export const baseQueryWithErrorHandling =  async(args: string | FetchArgs, api: BaseQueryApi, extraOptions: object) => {
    // start loading
    api.dispatch(startLoading());
    await sleep();
    const result = await  customBaseQuery(args, api, extraOptions);
    // stop loading
    api.dispatch(stopLoading());
    if (result.error) {
        const {status, data} = result.error;
        console.log({status, data});
        switch (status) {
            case 400:
            toast.error(data as string)
            break;
            case 401:
                toast.error(data as string || 'Unauthorized')
                break;
            default:
                break;
        }
    }
    return result;
}
