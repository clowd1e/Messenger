import { Message } from "../../../../models/message";
import { MessageDto } from "./message-dto";

export type MessageRenderItem = 
    | { type: 'message'; messageDto: MessageDto }
    | { type: 'date-badge'; label: string };