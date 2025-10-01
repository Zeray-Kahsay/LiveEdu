import type { UserDto } from "./userDto";

export interface AuthResponse {
  user: UserDto;
  accessToken: string;
  refreshToken: string;
}