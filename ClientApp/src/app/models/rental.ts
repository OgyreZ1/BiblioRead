import { Book } from './book';

export class Rental {
    id: number;
    userId: string;
    bookIds: number[];
    books: Book[];
    dateCreated: string;
    deadlineDate: string;
    isFinished: boolean;
}