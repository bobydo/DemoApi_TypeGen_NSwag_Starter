import { UserRole } from './user';

export class CreateUserRequest {
    name!: string;
    email!: string;
    role!: UserRole;
}
