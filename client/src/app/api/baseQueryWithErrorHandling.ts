import { fetchBaseQuery, type BaseQueryApi, type FetchArgs } from "@reduxjs/toolkit/query";
import { startLoading, stopLoading } from "../layout/uiSlice";
import { toast } from "react-toastify";
import { authApi } from "../../features/auth/authApi";

const customBaseQuery = fetchBaseQuery({
  baseUrl: 'http://localhost:50001/api',
  prepareHeaders: (headers) => {
    const token = localStorage.getItem('accessToken');
    if (token) headers.set('Authorization', `Bearer ${token}`);
    return headers;
  },
});

// optional delay to demo loading spinner
const sleep = (ms = 500) => new Promise(resolve => setTimeout(resolve, ms));

export const baseQueryWithErrorHandling = async (
  args: string | FetchArgs,
  api: BaseQueryApi,
  extraOptions: object
) => {
  api.dispatch(startLoading());
  await sleep(); // simulate latency

  let result = await customBaseQuery(args, api, extraOptions);

  if (result.error && result.error.status === 401) {
    // try refreshing token if 401
    const refreshToken = localStorage.getItem('refreshToken');
    const deviceId = localStorage.getItem('deviceId');
    if (refreshToken && deviceId) {
      try {
        const refreshResult: any = await api.dispatch(
          authApi.endpoints.refreshToken.initiate({ refreshToken, deviceId })
        ).unwrap();

        localStorage.setItem('accessToken', refreshResult.token);

        // retry original request
        result = await customBaseQuery(args, api, extraOptions);
      } catch {
        toast.error('Session expired. Please login again.');
      }
    } else {
      toast.error('Unauthorized. Please login.');
    }
  }

  // handle other errors
  if (result.error) {
    const { status, data } = result.error;
    switch (status) {
      case 400:
        toast.error(typeof data === 'string' ? data : 'Bad request');
        break;
      case 401:
        toast.error(typeof data === 'string' ? data : 'Unauthorized');
        break;
      case 500:
      default:
        toast.error('Internal server error');
        break;
    }
  }

  api.dispatch(stopLoading());
  return result;
};
