import { apiSlice } from "../../app/api/apiSlice";
import type { LoginDto } from "../../app/types/account/loginDto";
import type { RegisterDto } from "../../app/types/account/registerDto";
import type { UserDto } from "../../app/types/account/userDto";

export const authApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    registerUser: builder.mutation<UserDto, RegisterDto>({
      query: (user) => ({
        url: "/account/registerUser",
        method: "POST",
        body: user,
      }),
    }),
    loginUser: builder.mutation<UserDto, LoginDto>({
      query: (creds) => ({
        url: "/account/loginUser",
        method: "POST",
        body: creds,
      }),
    }),
    refreshToken: builder.mutation<{ token: string }, { refreshToken: string; deviceId: string }>({
      query: ({ refreshToken, deviceId }) => ({
        url: "/account/refreshToken",
        method: "POST",
        body: { refreshToken, deviceId },
      }),
    }),
  }),
  overrideExisting: true,
});

export const { useRegisterUserMutation, useLoginUserMutation, useRefreshTokenMutation } = authApi;
