import { useState, useEffect } from "react";

const useFetch = <T>(url: string) => {
    debugger;
    const [data, setData] = useState<T | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!url) return;

        const fetchData = async () => {
            try {
                const response = await fetch(url);
                if (!response.ok) throw new Error("Failed to fetch data");
                const result: T = await response.json();
                console.log(result);
                if (typeof result === "string") {
                    setData(null);
                } else {
                    setData(result as T);
                }
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [url]);

    return { data, loading, error };
};

export default useFetch;
