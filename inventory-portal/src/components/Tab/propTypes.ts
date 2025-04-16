import { ReactElement } from "react";

export interface TabProps {
    tabs: TabInterface[];
    defaultTab: string;
    screenLoader: (params: ScreenLoaderParams) => ReactElement | undefined;
}

export interface ScreenLoaderParams {
    screenId: string;
    RedirectToDefaultTab : () => void
}

export interface TabInterface {
    id: string;
    label: string;
    disabled?: boolean;
}