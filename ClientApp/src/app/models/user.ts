import { Rental } from './rental';

export class User {
    id: string;
    userName: string;
    fullName?: string;
    email?: string;
    role: string;
    rentals?: Rental[];
}