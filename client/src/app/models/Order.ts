import { Book } from "./Book";

export class Order{
    id: number;
    UserName: string;
    books: Book[]
    CreatedAt: Date;
    placed: boolean;
    processed: boolean;
    orderUrl: string;
}