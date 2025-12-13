export class User {
    id!: number;
    name!: string;
    email!: string;
    createdAt!: Date;
    role!: UserRole;
}

export enum UserRole {
    Admin = 0,
    User = 1,
    Guest = 2
}
