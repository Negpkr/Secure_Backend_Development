--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2 (Homebrew)
-- Dumped by pg_dump version 16.2 (Homebrew)

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
-- Name: artifact; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artifact (
    id integer NOT NULL,
    title character varying(255) NOT NULL,
    description text,
    creationdate date NOT NULL,
    artistid integer NOT NULL,
    exhibitionid integer NOT NULL
);


ALTER TABLE public.artifact OWNER TO postgres;

--
-- Name: artifact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.artifact_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.artifact_id_seq OWNER TO postgres;

--
-- Name: artifact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.artifact_id_seq OWNED BY public.artifact.id;


--
-- Name: artist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artist (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    biography text
);


ALTER TABLE public.artist OWNER TO postgres;

--
-- Name: artist_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.artist_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.artist_id_seq OWNER TO postgres;

--
-- Name: artist_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.artist_id_seq OWNED BY public.artist.id;


--
-- Name: exhibition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.exhibition (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    startdate date NOT NULL,
    enddate date NOT NULL
);


ALTER TABLE public.exhibition OWNER TO postgres;

--
-- Name: exhibition_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.exhibition_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.exhibition_id_seq OWNER TO postgres;

--
-- Name: exhibition_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.exhibition_id_seq OWNED BY public.exhibition.id;


--
-- Name: artifact id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artifact ALTER COLUMN id SET DEFAULT nextval('public.artifact_id_seq'::regclass);


--
-- Name: artist id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist ALTER COLUMN id SET DEFAULT nextval('public.artist_id_seq'::regclass);


--
-- Name: exhibition id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.exhibition ALTER COLUMN id SET DEFAULT nextval('public.exhibition_id_seq'::regclass);


--
-- Name: artifact artifact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artifact
    ADD CONSTRAINT artifact_pkey PRIMARY KEY (id);


--
-- Name: artist artist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist
    ADD CONSTRAINT artist_pkey PRIMARY KEY (id);


--
-- Name: exhibition exhibition_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.exhibition
    ADD CONSTRAINT exhibition_pkey PRIMARY KEY (id);


--
-- Name: artifact artifact_artistid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artifact
    ADD CONSTRAINT artifact_artistid_fkey FOREIGN KEY (artistid) REFERENCES public.artist(id);


--
-- Name: artifact artifact_exhibitionid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artifact
    ADD CONSTRAINT artifact_exhibitionid_fkey FOREIGN KEY (exhibitionid) REFERENCES public.exhibition(id);


--
-- PostgreSQL database dump complete
--

