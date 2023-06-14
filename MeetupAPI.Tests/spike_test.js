// This will tell how the system would perform during the spike
import { sleep } from 'k6';
import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '10s', target: 100 }, // below normal load
        { duration: '1m', target: 100 }, // warm up the service
        { duration: '10s', target: 1400 }, // fast spike to 1400 users
        { duration: '3m', target: 1400 }, // stay at that load for 3 mins
        { duration: '10s', target: 100 }, // scale down. Recovery stage
        { duration: '3m', target: 100 },
        { duration: '10s', target: 0 }, 
    ],
};

const API_BASE_URL = 'https://localhost:5001';
const name = '4Dev';

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/api/meetup?pageSize=50&pageNumber=1`],
        ['GET', `${API_BASE_URL}/api/meetup/${name}`],
    ]);

    sleep(1);
};