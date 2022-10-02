import { ProductionLineTank } from "./production-line-tank";
import { TankParts } from "./tank-parts";

export class TankModel {
  id: number;
  name: string;
  productionDays: number;
  active: boolean;
  description: string;

  parts: TankParts[];
  productionLines: ProductionLineTank[]

  showDetails: boolean = false;
}
