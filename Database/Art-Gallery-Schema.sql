--
-- PostgreSQL database dump
--

-- Dumped from database version 14.4
-- Dumped by pg_dump version 14.4

-- Started on 2022-12-05 16:09:15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 220 (class 1259 OID 65866)
-- Name: account; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.account (
    account_id integer NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    email text NOT NULL,
    password_hash text NOT NULL,
    role text NOT NULL,
    active_at timestamp without time zone NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT chk_email CHECK ((char_length(email) <= 255)),
    CONSTRAINT chk_first_name CHECK ((char_length(first_name) <= 100)),
    CONSTRAINT chk_last_name CHECK ((char_length(last_name) <= 100)),
    CONSTRAINT chk_password CHECK ((char_length(password_hash) <= 550)),
    CONSTRAINT chk_role CHECK ((char_length(role) <= 50))
);


ALTER TABLE public.account OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 65865)
-- Name: account_account_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.account ALTER COLUMN account_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.account_account_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 210 (class 1259 OID 65789)
-- Name: artist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artist (
    artist_id integer NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    display_name text NOT NULL,
    profile_image_url text NOT NULL,
    place_of_birth text,
    year_of_birth integer NOT NULL,
    year_of_death integer,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT chk_display_name CHECK ((char_length(display_name) <= 255)),
    CONSTRAINT chk_first_name CHECK ((char_length(first_name) <= 100)),
    CONSTRAINT chk_last_name CHECK ((char_length(last_name) <= 100)),
    CONSTRAINT chk_place_of_birth CHECK ((char_length(place_of_birth) <= 50)),
    CONSTRAINT chk_primary_image_url CHECK ((char_length(profile_image_url) <= 255))
);


ALTER TABLE public.artist OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 65788)
-- Name: artist_artist_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.artist ALTER COLUMN artist_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.artist_artist_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 215 (class 1259 OID 65828)
-- Name: artist_artwork; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artist_artwork (
    artist_id integer NOT NULL,
    artwork_id integer NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL
);


ALTER TABLE public.artist_artwork OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 65811)
-- Name: artwork; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artwork (
    artwork_id integer NOT NULL,
    title text NOT NULL,
    description text NOT NULL,
    media text NOT NULL,
    primary_image_url text NOT NULL,
    secondary_image_url text,
    year_created integer NOT NULL,
    nation_id integer NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT check_description CHECK ((char_length(description) <= 2500)),
    CONSTRAINT chk_media CHECK ((char_length(media) <= 150)),
    CONSTRAINT chk_primary_image_url CHECK ((char_length(primary_image_url) <= 255)),
    CONSTRAINT chk_secondary_image_url CHECK ((char_length(secondary_image_url) <= 255)),
    CONSTRAINT chk_title CHECK ((char_length(title) <= 255))
);


ALTER TABLE public.artwork OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 65810)
-- Name: artwork_artwork_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.artwork ALTER COLUMN artwork_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.artwork_artwork_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 218 (class 1259 OID 65852)
-- Name: artwork_exhibition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artwork_exhibition (
    artwork_id integer NOT NULL,
    exhibition_id integer NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL
);


ALTER TABLE public.artwork_exhibition OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 65842)
-- Name: exhibition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.exhibition (
    exhibition_id integer NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    background_image_url text NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT check_description CHECK ((char_length(description) <= 2500)),
    CONSTRAINT chk_background_image_url CHECK ((char_length(background_image_url) <= 255)),
    CONSTRAINT chk_name CHECK ((char_length(name) <= 2500))
);


ALTER TABLE public.exhibition OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 65841)
-- Name: exhibition_exhibition_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.exhibition ALTER COLUMN exhibition_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.exhibition_exhibition_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 212 (class 1259 OID 65802)
-- Name: nation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.nation (
    nation_id integer NOT NULL,
    title text NOT NULL,
    modified_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT chk_title CHECK ((char_length(title) <= 100))
);


ALTER TABLE public.nation OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 65801)
-- Name: nation_nation_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.nation ALTER COLUMN nation_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.nation_nation_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 3376 (class 0 OID 65866)
-- Dependencies: 220
-- Data for Name: account; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.account (account_id, first_name, last_name, email, password_hash, role, active_at, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3366 (class 0 OID 65789)
-- Dependencies: 210
-- Data for Name: artist; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.artist (artist_id, first_name, last_name, display_name, profile_image_url, place_of_birth, year_of_birth, year_of_death, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3371 (class 0 OID 65828)
-- Dependencies: 215
-- Data for Name: artist_artwork; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.artist_artwork (artist_id, artwork_id, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3370 (class 0 OID 65811)
-- Dependencies: 214
-- Data for Name: artwork; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.artwork (artwork_id, title, description, media, primary_image_url, secondary_image_url, year_created, nation_id, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3374 (class 0 OID 65852)
-- Dependencies: 218
-- Data for Name: artwork_exhibition; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.artwork_exhibition (artwork_id, exhibition_id, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3373 (class 0 OID 65842)
-- Dependencies: 217
-- Data for Name: exhibition; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.exhibition (exhibition_id, name, description, background_image_url, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3368 (class 0 OID 65802)
-- Dependencies: 212
-- Data for Name: nation; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.nation (nation_id, title, modified_at, created_at) FROM stdin;
\.


--
-- TOC entry 3382 (class 0 OID 0)
-- Dependencies: 219
-- Name: account_account_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.account_account_id_seq', 1, false);


--
-- TOC entry 3383 (class 0 OID 0)
-- Dependencies: 209
-- Name: artist_artist_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.artist_artist_id_seq', 1, false);


--
-- TOC entry 3384 (class 0 OID 0)
-- Dependencies: 213
-- Name: artwork_artwork_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.artwork_artwork_id_seq', 1, false);


--
-- TOC entry 3385 (class 0 OID 0)
-- Dependencies: 216
-- Name: exhibition_exhibition_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.exhibition_exhibition_id_seq', 1, false);


--
-- TOC entry 3386 (class 0 OID 0)
-- Dependencies: 211
-- Name: nation_nation_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.nation_nation_id_seq', 1, false);


--
-- TOC entry 3220 (class 2606 OID 65877)
-- Name: account account_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.account
    ADD CONSTRAINT account_pkey PRIMARY KEY (account_id);


--
-- TOC entry 3212 (class 2606 OID 65800)
-- Name: artist artist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist
    ADD CONSTRAINT artist_pkey PRIMARY KEY (artist_id);


--
-- TOC entry 3216 (class 2606 OID 65822)
-- Name: artwork artwork_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artwork
    ADD CONSTRAINT artwork_pkey PRIMARY KEY (artwork_id);


--
-- TOC entry 3218 (class 2606 OID 65851)
-- Name: exhibition exhibition_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.exhibition
    ADD CONSTRAINT exhibition_pkey PRIMARY KEY (exhibition_id);


--
-- TOC entry 3214 (class 2606 OID 65809)
-- Name: nation nation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nation
    ADD CONSTRAINT nation_pkey PRIMARY KEY (nation_id);


--
-- TOC entry 3222 (class 2606 OID 65831)
-- Name: artist_artwork fk_artist; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist_artwork
    ADD CONSTRAINT fk_artist FOREIGN KEY (artist_id) REFERENCES public.artist(artist_id);


--
-- TOC entry 3223 (class 2606 OID 65836)
-- Name: artist_artwork fk_artwork; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist_artwork
    ADD CONSTRAINT fk_artwork FOREIGN KEY (artwork_id) REFERENCES public.artwork(artwork_id);


--
-- TOC entry 3224 (class 2606 OID 65855)
-- Name: artwork_exhibition fk_artwork; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artwork_exhibition
    ADD CONSTRAINT fk_artwork FOREIGN KEY (artwork_id) REFERENCES public.artwork(artwork_id);


--
-- TOC entry 3225 (class 2606 OID 65860)
-- Name: artwork_exhibition fk_exhibition; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artwork_exhibition
    ADD CONSTRAINT fk_exhibition FOREIGN KEY (exhibition_id) REFERENCES public.exhibition(exhibition_id);


--
-- TOC entry 3221 (class 2606 OID 65823)
-- Name: artwork fk_nation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artwork
    ADD CONSTRAINT fk_nation FOREIGN KEY (nation_id) REFERENCES public.nation(nation_id);


-- Completed on 2022-12-05 16:09:15

--
-- PostgreSQL database dump complete
--

