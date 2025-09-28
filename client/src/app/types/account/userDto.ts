export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  schoolName: string;
  token?: string;
  roles: string[];
  refreshToken?: string;
}   