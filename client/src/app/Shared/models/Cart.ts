import { Product } from "./Product";
import { v4 as uuidv4 } from 'uuid';

export type CartType = {
  id: string;
  items: Product[];
};

export class Cart implements CartType {
  id = uuidv4();
  items: Product[] = [];
}
