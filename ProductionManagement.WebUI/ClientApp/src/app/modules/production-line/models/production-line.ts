import { LineTank } from "./line-tank";

export class ProductionLine {
  id: number;
  name: string;
  active: boolean;
  tanks: LineTank[];

  showDetails: boolean;
}
