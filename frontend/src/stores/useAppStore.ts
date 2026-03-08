import { create } from "zustand";
import { devtools } from "zustand/middleware";
import { createUiSlice, type UiSliceType } from "./uiSlice";

export const useAppStore = create<UiSliceType>()(
  devtools((...args) => ({
    ...createUiSlice(...args),
  })),
);
