export class OrderModel {
  id: number;
  orderName: string;
  description: string;
  tankId: number;
  tankName: string;
  productionDays: number;
  productionLinesNames: string;
  color: string;
  sequence: number;
  startDate?: Date;
  stopDate?: Date;

  checked: boolean;
}
