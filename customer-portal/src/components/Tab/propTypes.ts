import { ReactElement } from "react";

export interface TabProps {
    tabs: TabInterface[];
    defaultTab: string;
    screenLoader: (params: ScreenLoaderParams) => ReactElement | undefined;
}

export interface ScreenLoaderParams {
    screenId: string;
}

export interface TabInterface {
    id: string;
    label: string;
    disabled?: boolean;
}